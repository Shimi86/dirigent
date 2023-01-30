﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirigent
{

	public class IPPort
	{
		public string IP = "";
		public int Port;
	}
		
    
	public class GatewaySession : Disposable, ISshStateProvider
	{
		public static string DirigentServiceName = "DIRIGENT";

        GatewayDef _def;
		string _masterIP; 
		int _masterPort; // local behind the gateway
		List<Machine> _machines = new();

		PortForwarder? _portFwd;

		public bool IsConnected => _portFwd is null ? false : _portFwd.IsRunning;
		public GatewayDef Gateway => _def;

		/// <summary> behind the gateway </summary>
		public string MasterIP => _masterIP;
		/// <summary> behind the gateway </summary>
		public int MasterPort => _masterPort;

		public GatewaySession( GatewayDef def, string masterIP, int masterPort )
		{
            _def = def;
			_masterIP = masterIP;
			_masterPort = masterPort;

			PrepareMachines();
			PrepareDirigentMasterPortMapping();
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
			if( !disposing ) return;

            Close();
        }

		public void Open()
		{
			if( _portFwd is null )
			{
				_portFwd = new PortForwarder( _def, _machines );
				_portFwd.Start();
			}
		}

		public void Close()
		{
			_portFwd?.Dispose();
		}

		public bool AreMachinesSame( IEnumerable<MachineDef> machines )
		{
			return _def.Machines.SequenceEqual( machines );
		}

		public class Machine
        {
			public string Id = "";
            public string IP = "";
			public bool AlwaysLocal;
            public List<Service> Services = new List<Service>();
        }


		void PrepareMachines()
		{
			_machines.Clear();
			
            foreach( var machDef in _def.Machines )
            {
				var m = new Machine();
				m.Id = machDef.Id;
				m.IP = machDef.IP;
				m.AlwaysLocal = machDef.AlwaysLocal;
				foreach ( var svcConf in machDef.Services )
                {
                    m.Services.Add(
                        new Service(
                            svcConf.Name,
                            
                            // local
                            machDef.IP,
                            svcConf.Port,
                            
                            // remote
                            "127.0.0.1",
                            ++GatewayManager.LocalPortBase
                        )
                        {
                        }
                    );
                }
				_machines.Add( m );
			}

		}

		void PrepareDirigentMasterPortMapping()
		{
			// add map for dirigent master
			// search for the machine with the same IP as the master
			var masterMachine = _machines.Find( m => m.IP == _masterIP );
			if( masterMachine is null )
			{
				masterMachine = new Machine();
				masterMachine.Id = "DIRIGENT_MASTER";
				masterMachine.IP = _masterIP;
				_machines.Add( masterMachine );
			}
			// search for service with the same port as the master
			var masterService = masterMachine.Services.Find( s => s.LocalPort == _masterPort );
			if (masterService is null)
			{
				masterService = new Service(
					DirigentServiceName,
					masterMachine.IP,
					_masterPort,
					"127.0.0.1",
					++GatewayManager.LocalPortBase
				);
				masterMachine.Services.Add( masterService );
			}
		}

		IPPort? GetPortMap( Machine mach, string serviceName )
		{
			var svc = mach.Services.Find( s => string.Equals(s.Name, serviceName, StringComparison.OrdinalIgnoreCase) );
			if( svc is null)
				return null;

			return new IPPort()
			{
				IP = svc.GetIP( IsConnected ),
				Port = svc.GetPort( IsConnected )
			};
		}

		public IPPort? GetPortMapByMachineName( string machineId, string serviceName )
		{
			var mach = _machines.Find( m => string.Equals(m.Id, machineId, StringComparison.OrdinalIgnoreCase) );
			if( mach is null )
				return null;

			return GetPortMap( mach, serviceName );
		}

		public IPPort? GetPortMapByMachineIP( string machineIP, string serviceName )
		{
			var mach = _machines.Find( m => string.Equals(m.IP, machineIP, StringComparison.OrdinalIgnoreCase) );
			if( mach is null )
				return null;

			return GetPortMap( mach, serviceName );
		}

		// returns null if variables can't be resolved (machine not found etc.)
		public Dictionary<string, string> GetVariables( string machineId, string serviceName )
		{
			var vars = new Dictionary<string, string>();

			var comp = _machines.Find( m => string.Equals(m.Id, machineId, StringComparison.OrdinalIgnoreCase) );
			if (comp is null)
				return vars;

			
			bool remote = IsConnected;
			
			// set service-related variables
			var svc = comp.Services.Find( (x) => x.Name == serviceName );
            if( svc != null )
            {
                vars["SVC_IP"] = svc.GetIP( remote );
                vars["SVC_PORT"] = svc.GetPort( remote ).ToString();
                //vars["SVC_USERNAME"] = svc.UserName;
                //vars["SVC_PASSWORD"] = svc.Password;
            }

            vars["GW_IP"] = IsConnected ? _def.ExternalIP : _def.InternalIP;
            vars["GW_PORT"] = _def.Port.ToString();
            vars["GW_USERNAME"] = _def.UserName;
            vars["GW_PASSWORD"] = _def.Password;

            vars["APP_NEW_GUID"] = Guid.NewGuid().ToString();

			return vars;

		}
	}
}
