using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
using Dirigent;

public class DemoScript1 : Script
{
	private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType );

	bool _started_m1_a = false;
	bool _started_m1_b = false;

	public override void Init()
	{
		log.Info($"Init with args: '{Args}'");
		StatusText = "Initialized";
	}

	public override void Done()
	{
		log.Info("Done!");
		// kill what was started by us
		if( _started_m1_a )
			KillApp("m1.a");

		if( _started_m1_b )
			KillApp("m1.b");

		StatusText = "Finished";
	}

	public async override Task Run( CancellationToken ct )
	{
		log.Info("Run!");

		StatusText = "Waiting for m1 to boot";

		// wait for agent m1 to boot
		while( GetClientState("m1") is null )
		{
			if( ct.IsCancellationRequested )return;
			await Task.Delay(100);
		}

		// start app "m1.a" defined within "plan1"
		StartApp( "m1.a", "plan1" );
		_started_m1_a = true;
		
		// wait for the app to initialize
		StatusText = "Waiting for m1.a to initialize";
		while ( !GetAppState( "m1.a" ).Initialized )
		{
			if( ct.IsCancellationRequested ) return;
			await Task.Delay(100);
		}

		// start app "m1.b" defined within "plan1"
		StartApp( "m1.b", "plan1" );
		_started_m1_b = true;

		// both apps should be killed in Done() once the coroutine terminates and the script gets disposed

		//yield return new WaitForSeconds(2);
		//KillApp("m1.a");
		
		//yield return new WaitForSeconds(2);
		//KillApp("m1.b");
		
		StatusText = "Waiting before terminating";
		await Task.Delay(4000);
	}
}
