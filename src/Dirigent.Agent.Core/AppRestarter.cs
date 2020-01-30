﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirigent.Common;
using System.Xml.Linq;

using X = Dirigent.Common.XmlConfigReaderUtils;

namespace Dirigent.Agent.Core
{
	
	/// <summary>
	/// Instantiated for a conrete app, on the agent where the associated app is local,
	/// and just for one single restart operation (removed after each restart).
	/// Waits until the app disappears, then starts it again and deactivates itself.
	/// Counts the remaining number of restarts (stored in appState).
	/// Once a limit is reached, deactivates itself (marks for removal).
	/// When a new restarter is added, it can either continue the counting down
	/// the number of restarts or reset the number of restarts to the AppDef-configured value.
	/// </summary>
	public class AppRestarter
	{
		/// <summary>
		/// Done restating, can be removed from the system.
		/// </summary>
		public bool ShallBeRemoved { get; protected set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
        AppDef appDef;
        AppState appState; // reference to a live writable appState - we will modify it here

        enum eState 
        {
            Init,
			WaitingForDeath, // app is terminating
			WaitBeforeRestart,
            WaitingBeforeRestart, // giving some time before we restart it
			Restart,
			Disabled
        };

        eState state;

        DateTime waitingStartTime;
		LocalOperations localOps;
		bool waitBeforeRestart;
		
        double RESTART_DELAY = 1.0; // how long to wait before restarting the app
        int MAX_TRIES = -1; // how many times to try restarting before giving up; -1 = forever

		public AppRestarter( AppDef appDef, AppState appState, LocalOperations localOps, bool waitBeforeRestart )
		{
            this.appDef = appDef;
		    this.appState = appState;
			this.localOps = localOps;

            parseXml();

			Reset(waitBeforeRestart);
        }

        void parseXml()
        {
			XElement xml = null;
			if( !String.IsNullOrEmpty( appDef.RestarterXml ) )
			{
				var rootedXmlString = String.Format("<root>{0}</root>", appDef.RestarterXml);
				var xmlRoot = XElement.Parse(rootedXmlString);
				xml = xmlRoot.Element("Restarter");
			}

			if( xml == null ) return;
			
			RESTART_DELAY = X.getDoubleAttr(xml, "delay", RESTART_DELAY, true);
			MAX_TRIES = X.getIntAttr(xml, "maxTries", MAX_TRIES, true);
        }



        public void Tick()
        {
            switch( state )
            {
                case eState.Init:
                {
					appState.Restarting = true;

                    if( appState.Running )
                    {
                        log.DebugFormat("AppRestarter: Waiting for app to die appid {0}", appDef.AppIdTuple );
						state = eState.WaitingForDeath;
                    }
					else
					{
						if( waitBeforeRestart )
						{
							state = eState.WaitBeforeRestart;
						}
						else
						{
							// go right to starting
							state = eState.Restart;
						}
					}
                    break;
                }

                case eState.WaitingForDeath:
                {
                    // has the application terminated?
                    if( !appState.Running )
                    {
                        state = eState.WaitBeforeRestart;
                        waitingStartTime = DateTime.Now;

                    }
                    break;
                }

                case eState.WaitBeforeRestart:
                {
                    log.DebugFormat("AppRestarter: Waiting before restart appid {0}", appDef.AppIdTuple );
                    waitingStartTime = DateTime.Now;
                    state = eState.WaitingBeforeRestart;
					break;
				}
                case eState.WaitingBeforeRestart:
                {
                    var waitTime = (DateTime.Now - waitingStartTime).TotalSeconds;
                    if( waitTime > RESTART_DELAY )
                    {
                        state = eState.Restart;
                    }
                    break;
                }

                case eState.Restart:
                {
					bool launch = false;

					if( appState.RestartsRemaining == AppState.RESTARTS_UNLIMITED )
					{
						launch = true;
					}
					else
					// if < 0, don't limit the number of restarts...
					if( appState.RestartsRemaining > 0 )
					{
						appState.RestartsRemaining--;
						launch = true;
					}

					if( launch )
					{
						// start the app again (and leave the number of restarts as is)
						localOps.LaunchAppInternal( appDef.AppIdTuple, false );
					}
					
					// deactivate itself
					appState.Restarting = false;
	                ShallBeRemoved = true;
					state = eState.Disabled;

					break;
				}
				
				case eState.Disabled:
				{
					// do nothing
					break;
				}
            }

        }

		/// <summary>
		/// Starts watching for death from the beginning
		/// </summary>
		public void Reset(bool waitBeforeRestart)
		{
			this.waitBeforeRestart = waitBeforeRestart;

			if( appState.RestartsRemaining == AppState.RESTARTS_UNITIALIZED )
			{
				InitReseToMax();
			}
			else
			{
				InitContinue();
			}
		}
		
		void InitReseToMax()
		{
			// reset to max remaining tries
			appState.RestartsRemaining = MAX_TRIES;

			InitContinue();
		}

		void InitContinue()
		{
			if( appState.RestartsRemaining == 0 ) // no more tries, deactivate itself
			{
				ShallBeRemoved = true;
				state = eState.Disabled;
			}
			else  // let's continue waiting for death and restartin
			{
				ShallBeRemoved = false;
				state = eState.Init;
			}
		}

		public void Dispose()
		{
			// Make sure we don't leave the restarting flag on the app if we are removed in the middle
			// of operation
			appState.Restarting = false;
		}

	}
}
