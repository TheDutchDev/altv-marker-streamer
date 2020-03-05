# ALT:V MP Server-side Marker Streamer
A server-side C# implementation of an marker streamer for ALT:V MP.

## Installation
- This resource makes use of the ``AltV.Net.EntitySync (v1.0.20-dev-preview)`` and ``AltV.Net.EntitySync.ServerEvent (v1.0.19-dev-preview)`` nuget package, make sure to install those prior to using this resource.
- Copy ``server-scripts/MarkerStreamer.cs`` to your gamemode.
- Make sure to add the following code to your gamemode's OnStart() method(the marker streamer won't work without it!):
```csharp
// Documentation: https://fabianterhorst.github.io/coreclr-module/articles/entity-sync.html
AltEntitySync.Init( 1, 100,
   repository => new ServerEventNetworkLayer( repository ),
   ( ) => new LimitedGrid3( 50_000, 50_000, 100, 10_000, 10_000, 600 ),
   new IdProvider( )
);
```
- Copy ``marker-streamer-client`` to your ``server-root/resources`` directory.
- Add ``"marker-streamer-client"`` to your server config resources list.

## Usage
The following global methods are available:
```csharp
// Create a new marker on the map, returns the created marker.
DynamicMarker CreateDynamicMarker(
    MarkerTypes markerType, Vector3 position, Vector3 scale, Vector3 ? rotation = null, Vector3? direction = null, Rgba? color = null, 
    bool? bobUpDown = false, bool? faceCamera = false, bool? rotate = false, string textureDict = null, string textureName = null, 
    bool? drawOnEnter = false, int dimension = 0, uint streamRange = 100
);

// Destroy an marker by it's ID or marker instance. returns true if successful.
bool DestroyDynamicMarker( ulong dynamicMarkerId );
void DestroyDynamicMarker( DynamicMarker marker );

// Get an marker by it's ID. returns the marker if successful or null if not.
DynamicMarker GetDynamicMarker( ulong dynamicMarkerId );

// Destroy all created markers.
void DestroyAllDynamicMarkers( );

// Get a list of all created markers.
List<DynamicMarker> GetAllDynamicMarkers( );
```

Each marker has it's own set of methods and properties that can be used:
```csharp
// Get/set marker's rotation.
Vector3 Rotation { get; set; }

// Get/set marker's position.
Vector3 Position { get; set; }

// Get/set marker's texture dictionary
string TextureDict { get; set; }

// Get/set marker's texture name.
string TextureName { get; set; }

// Get/set whether the marker should rotate on the Y axis(heading).
bool? Rotate { get; set; }

// Get/set whether the marker should be drawn onto the entity when they enter it.
bool? DrawOnEnter { get; set; }

// Get/set whether the marker should rotate on the Y axis towards the player's camera.
bool? FaceCamera { get; set; }

// Get/set whether the marker should bob up and down.
bool? BobUpDown { get; set; }

// Get/set scale of the marker.
Vector3 Scale { get; set; }

// Get/set - Represents a heading on each axis in which the marker should face, alternatively you can rotate each axis independently with Rotation and set Direction axis to 0.
Vector3 Direction { get; set; }

// Get/set the current marker's type(see MarkerTypes enum).
MarkerTypes MarkerType { get; set; }

// Get/set marker color. (default white)
Rgba? Color { get; set; }

// Destroy the marker and all it's data.
void Destroy( );
```

## Examples
```csharp
// Create a marker.
DynamicMarker marker = MarkerStreamer.CreateDynamicMarker( MarkerTypes.MarkerTypeVerticalCylinder, new Vector3( -879.655f, -853.499f, 19.566f ), new Vector3( 1 ), color: new Rgba( 125, 52, 21, 255 ) );

// Change marker type into plane model.
marker.MarkerType = MarkerTypes.MarkerTypePlaneMode;

// Change position.
marker.Position = new Position( 300f, 500f, 25f ); // Accepts both Vector3 and Position types.

// Change rotation.
marker.Rotation = new Rotation( 0f, 0f, 25f ); // Accepts both Vector3 and Rotation types.

// Set an marker's color
marker.LightColor = new Rgba( 25, 49, 120, 255 ); // random

// Destroy the marker
MarkerStreamer.DestroyDynamicMarker( marker ); // has an overload method that accepts an ID instead of marker instance.
```

Furthermore, there's an example C# file included in the package, the example file can be found at ``server-scripts/ExampleServerMarkers.cs``.