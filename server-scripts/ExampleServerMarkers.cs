using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using DasNiels.AltV.Streamers;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace TestServer
{
    public class ExampleServerMarkers : AsyncResource
    {
        public override void OnStart( )
        {
            // YOU MUST ADD THIS IN THE ONSTART OF YOUR GAMEMODE, OBJECTSTREAMER WONT WORK WITHOUT IT!
            AltEntitySync.Init( 1, 100,
               repository => new ServerEventNetworkLayer( repository ),
               ( ) => new LimitedGrid3( 50_000, 50_000, 100, 10_000, 10_000, 600 ),
               new IdProvider( ) 
            );
            //////////////////////////

            AltAsync.OnPlayerConnect += OnPlayerConnect;
            AltAsync.OnConsoleCommand += OnConsoleCommand;

            // Spawn markers
            CreateMarkers( );

            // Display commands in console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine( "|---------------------AVAILABLE CONSOLE COMMANDS:---------------------|" );
            Console.WriteLine( "| dam -> Destroy all created markers." );
            Console.WriteLine( "| cam -> Create all markers defined in the CreateMarkers method." );
            Console.WriteLine( " " );
            Console.WriteLine( "| cp {id} -> Move a specified marker by 5 units on the Z axis(height)." );
            Console.WriteLine( "| cr {id} -> Rotate a specified marker by 5 units on the Z axis(yaw)." );
            Console.WriteLine( "| ct {id} -> Change type of the specified marker." );
            Console.WriteLine( "| cd {id} -> Change direction of the specified marker." );
            Console.WriteLine( " " );
            Console.WriteLine( "| do {id} -> Destroy a dynamic marker by ID(IDs start at 0)." );
            Console.WriteLine( "| go {id} -> Get dynamic marker data of the specified marker ID." );
            Console.WriteLine( " " );
            Console.WriteLine( "| countmarker -> Get the amount of created markers." );
            Console.WriteLine( "|--------------------------------------------------------------------|" );
            Console.ResetColor( );
        }

        private void CreateMarkers( )
        {
            // Create some markers
            MarkerStreamer.CreateDynamicMarker( MarkerTypes.MarkerTypeVerticalCylinder, new Vector3( -879.655f, -853.499f, 19.566f ), new Vector3( 1 ), color: new Rgba( 125, 52, 21, 255 ) );
            MarkerStreamer.CreateDynamicMarker( MarkerTypes.MarkerTypePlaneModel, new Vector3( -869.655f, -853.499f, 19.566f ), new Vector3( 1 ), color: new Rgba( 25, 231, 125, 255 ) );
            MarkerStreamer.CreateDynamicMarker( MarkerTypes.MarkerTypeChevronUpx2, new Vector3( -859.655f, -853.499f, 19.566f ), new Vector3( 1 ), color: new Rgba( 125, 10, 250, 255 ) );
        }

        private async Task OnConsoleCommand( string name, string[ ] args )
        {
            // destroy all markers
            if( name == "dao" )
            {
                MarkerStreamer.DestroyAllDynamicMarkers( );
                Console.WriteLine( $"all markers destroyed." );
            }

            // create all markers
            if( name == "cao" )
            {
                MarkerStreamer.DestroyAllDynamicMarkers( );
                CreateMarkers( );
            }

            // destroy marker
            if( name == "do" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                if( MarkerStreamer.DestroyDynamicMarker( markerId ) )
                {
                    Console.WriteLine( $"Marker with ID { markerId } deleted!" );
                }
            }

            // change rotation
            if( name == "cr" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                var marker = MarkerStreamer.GetDynamicMarker( markerId );
                if( marker != null )
                {
                    Vector3 rot = marker.Rotation;
                    marker.Rotation = new Vector3( rot.X, rot.Y, rot.Z + 5f );
                    Console.WriteLine( $"Marker rotation increased on Z with +5f" );
                }
                else
                    Console.WriteLine( $"Couldnt find marker with ID { markerId }" );
            }

            // change direction
            if( name == "cd" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                var marker = MarkerStreamer.GetDynamicMarker( markerId );
                if( marker != null )
                {
                    Vector3 dir = marker.Direction;
                    marker.Direction = new Vector3( dir.X, dir.Y, dir.Z + 5f );
                    Console.WriteLine( $"Marker direction increased on Z with +5f" );
                }
                else
                    Console.WriteLine( $"Couldnt find marker with ID { markerId }" );
            }

            // change type
            if( name == "ct" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                var marker = MarkerStreamer.GetDynamicMarker( markerId );
                if( marker != null )
                {
                    var variations = Enum.GetValues( typeof( MarkerTypes ) );

                    marker.MarkerType = ( MarkerTypes ) variations.GetValue( new Random( ).Next( variations.Length ) );
                    Console.WriteLine( $"Marker type changed to a random type" );
                }
                else
                    Console.WriteLine( $"Couldnt find marker with ID { markerId }" );
            }

            // change pos
            if( name == "cp" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                var marker = MarkerStreamer.GetDynamicMarker( markerId );
                if( marker != null )
                {
                    Console.WriteLine( $"marker pos: { marker.Position.Z }" );

                    marker.Position += new Vector3( 0, 0, 5 );
                    Console.WriteLine( $"Marker position increased on Z with +5f { marker.Position.Z }" );
                }
                else
                    Console.WriteLine( $"Couldnt find marker with ID { markerId }" );
            }

            // get marker by ID
            if( name == "go" )
            {
                if( args.Length == 0 )
                    return;

                ulong markerId = Convert.ToUInt64( args[ 0 ] );
                var marker = MarkerStreamer.GetDynamicMarker( markerId );
                if( marker != null )
                {
                    Console.WriteLine( $"Marker found, data: { marker.MarkerType }, { marker.Rotation.X }, { marker.Rotation.Y }, { marker.Rotation.Z }, ...!" );
                }
                else
                    Console.WriteLine( $"Couldnt find marker with ID { markerId }" );
            }

            // count markers
            if( name == "countmarker" )
            {
                Console.WriteLine( $"total markers created: { MarkerStreamer.GetAllDynamicMarkers( ).Count }" );
            }
        }

        private async Task OnPlayerConnect( IPlayer player, string reason )
        {
            Console.WriteLine( $"{ player.Name } connected!" );
            player.Model = ( uint ) AltV.Net.Enums.PedModel.FreemodeMale01;
            player.Spawn( new Position( -889.655f, -853.499f, 20.566f ), 0 );
        }

        public override void OnStop( )
        {
            MarkerStreamer.DestroyAllDynamicMarkers( );
            Console.WriteLine( $"Server stopped." );
        }
    }
}
