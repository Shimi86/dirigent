﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Linq;

using Dirigent.Common;

using X = Dirigent.Common.XmlConfigReaderUtils;
using System.Diagnostics;

namespace Dirigent.Common
{
    
    public class SharedXmlConfigReader
    {

        SharedConfig cfg;
        XDocument doc;

        public SharedConfig Load( System.IO.TextReader textReader )
        {
            cfg = new SharedConfig();
            doc = XDocument.Load(textReader);

            
            loadPlans();
            //loadMachines();
            //loadMaster();

            return cfg;
        }

        AppDef readAppElement( XElement e )
        {
            AppDef a;

            // first load templates
            var templateName = X.getStringAttr(e, "Template");
            if( templateName != "" )
            {
                XElement te = (from t in doc.Element("Shared").Elements("AppTemplate")
                        where (string) t.Attribute("Name") == templateName
                        select t).First();

                a = readAppElement( te );
            }
            else
            {
                a = new AppDef();
            }
            
            // read element content into memory, apply defaults
            var x = new {
                AppIdTuple = (string) e.Attribute("AppIdTuple"),
                ExeFullPath = (string) e.Attribute("ExeFullPath"),
                StartupDir = (string) e.Attribute("StartupDir"),
                CmdLineArgs = (string) e.Attribute("CmdLineArgs"),
                StartupOrder = (string) e.Attribute("StartupOrder") ?? "0",
                RestartOnCrash = (string) e.Attribute("RestartOnCrash") ?? "0",
                InitCondition = (string) e.Attribute("InitCondition"),
                SeparationInterval = (string) e.Attribute("SeparationInterval") ?? "0.0",
                Dependecies = (string) e.Attribute("Dependencies"),
                KillTree = (string)e.Attribute("KillTree"),
                WindowStyle = (string)e.Attribute("WindowStyle"),
            };

            // then overwrite templated values with current content
            if( x.AppIdTuple != null ) a.AppIdTuple = new AppIdTuple( x.AppIdTuple );
            if( x.ExeFullPath != null ) a.ExeFullPath = x.ExeFullPath;
            if( x.StartupDir != null ) a.StartupDir = x.StartupDir;
            if( x.CmdLineArgs != null ) a.CmdLineArgs = x.CmdLineArgs;
            if( x.StartupOrder != null ) a.StartupOrder = int.Parse( x.StartupOrder );
            if( x.RestartOnCrash != null ) a.RestartOnCrash = (int.Parse( x.RestartOnCrash ) != 0);
            if( x.InitCondition != null ) a.InitializedCondition = x.InitCondition;
            if( x.SeparationInterval != null ) a.SeparationInterval = double.Parse(x.SeparationInterval, CultureInfo.InvariantCulture );
            if (x.Dependecies != null)
            {
                var deps = new List<string>();
                foreach( var d in x.Dependecies.Split(';'))
                {
                    var stripped = d.Trim();
                    if( stripped != "" )
                    {
                        deps.Add( d );
                    }

                }
                a.Dependencies = deps;
            }

            if (x.KillTree != null) a.KillTree = (int.Parse(x.KillTree) != 0);
            
            if (x.WindowStyle != null)
            {
                if (x.WindowStyle.ToLower() == "minimized") a.WindowStyle = ProcessWindowStyle.Minimized;
                else
                if (x.WindowStyle.ToLower() == "maximized") a.WindowStyle = ProcessWindowStyle.Maximized;
                else
                if (x.WindowStyle.ToLower() == "normal") a.WindowStyle = ProcessWindowStyle.Normal;
                else
                if (x.WindowStyle.ToLower() == "hidden") a.WindowStyle = ProcessWindowStyle.Hidden;
            }

            return a;
        }

        void loadPlans()
        {
            var plans = from e in doc.Element("Shared").Descendants("Plan")
                         select e;

            foreach( var p in plans )
            {
                var planName = (string) p.Attribute("Name");

                var apps = from e in p.Descendants("App")
                            select readAppElement( e );
                
                // check if everything is valid
                int index = 1;
                foreach( var a in apps )
                {
                    if( a.AppIdTuple == null )
                    {
                        throw new ConfigurationErrorException(string.Format("App #{0} in plan '{1}' not having valid AppTupleId", index, planName));
                    }

                    if( a.ExeFullPath == null )
                    {
                        throw new ConfigurationErrorException(string.Format("App #{0} in plan '{1}' not having valid ExeFullPath", index, planName));
                    }

                    index ++;
                }
                
                cfg.Plans.Add(
                    new LaunchPlan(
                        planName,
                        new List<AppDef>( apps )
                    )
                );
            }

        }

        //MachineDef readMachineElement( XElement e )
        //{
        //    MachineDef m = new MachineDef();
        //    m.MachineId = X.getStringAttr(e, "Name");
        //    m.IpAddress = X.getStringAttr(e, "IpAddress");
        //    return m;
        //}

        //void loadMachines()
        //{
        //    var machines = from m in doc.Element("Shared").Descendants("Machine")
        //                 select readMachineElement(m);
            
        //    foreach( var ma in machines )
        //    {
        //        cfg.Machines.Add( ma.MachineId, ma );
        //    }
        //}

        //void loadMaster()
        //{
        //    var master = doc.Element("Shared").Element("Master");
        //    cfg.MasterPort = X.getIntAttr( master, "Port" );
        //    cfg.MasterName = X.getStringAttr( master, "Name" );
        //}

        //void loadLocalMachineId()
        //{
        //    var master = doc.Element("Shared").Element("Local");
        //    cfg.MasterPort = X.getIntAttr( master, "MasterPort" );
        //    cfg.MasterName = X.getStringAttr( master, "MasterName" );
        //}

    }
}
