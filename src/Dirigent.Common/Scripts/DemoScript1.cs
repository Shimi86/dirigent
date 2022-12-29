using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Dirigent;

public class DemoScript1 : Script
{
	private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType );

	//[MessagePack.MessagePackObject]
	public class Result
	{
		//[MessagePack.Key( 1 )]
		public int Code;

		public override string ToString() => Code.ToString();
	}

	protected async override Task<byte[]?> Run( CancellationToken ct )
	{
		if( TryGetArgs<string>( out var strArgs ) )
		{
			log.Info($"Init with string args: '{strArgs}'");
		}

		log.Info("Run!");

		SetStatus("Waiting for m1 to boot");

		//// wait for agent m1 to boot
		//while( await Dirig.GetClientStateAsync("m1") is null ) await Task.Delay(100, ct);

		// start app "m1.a" defined within "plan1"
		await StartApp( "m1.a", "plan1" );
		
		// wait for the app to initialize
		SetStatus("Waiting for m1.a to initialize");
		while ( !(await GetAppState( "m1.a" ))!.Initialized ) await Task.Delay(100, ct);

		// start app "m1.b" defined within "plan1"
		await StartApp( "m1.b", "plan1" );

		await Task.Delay(2000, ct);

		// run action on where this script was issued from
		await RunAction(
			Requestor,	// we are starting the action on behalf of the original requestor of this script
			new ToolActionDef { Name= "Notepad", Args="C:/Request/From/DemoScript1.cs" },
			Requestor // we want the action to run on requestor's machine
		);

		//SetStatus("Waiting before throwing exception");
		//await Task.Delay(4000, ct);
		//throw new Exception( "Demo exception" );

		SetStatus("Waiting before terminating");
		await Task.Delay(4000, ct);

		await KillApp( "m1.a" );
		await KillApp( "m1.b" );

		return MakeResult( new Result { Code = 17 } );
	}
}
