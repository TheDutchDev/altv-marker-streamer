/*
    Developed by DasNiels/Niels/DingoDongBlueBalls
*/

import * as alt from 'alt';

import { markerStreamer } from "./marker-streamer";

// when an object is streamed in
alt.onServer( "entitySync:create", entity => {
    
    if( entity.data ) {
        let data = entity.data;

        if( data && data.entityType === "marker" ) {

            markerStreamer.addMarker(
                +entity.id, data.markerType, data.entityType,
                entity.position, data.rotation,
                data.direction, data.scale, data.color, data.bobUpDown, data.faceCam, data.rotate, data.textureDict, data.textureName, data.drawOnEnter
            );
        }
    }
    else
    {
        markerStreamer.restoreMarker( +entity.id );
    }
} );

// when an object is streamed out
alt.onServer( "entitySync:remove", entityId => {
    markerStreamer.removeMarker( +entityId );
} );

// when a streamed in object changes position data
alt.onServer( "entitySync:updatePosition", ( entityId, position ) => {
    let marker = markerStreamer.getMarker( +entityId );

    if( marker === null )
        return;

    markerStreamer.setPosition( marker, position );
} );

// when a streamed in object changes data
alt.onServer( "entitySync:updateData", ( entityId, newData ) => {

    alt.log( "data: ", JSON.stringify( newData ) );

    let marker = markerStreamer.getMarker( +entityId );

    if( marker === null )
        return;

    if( newData.hasOwnProperty( "rotation" ) )
        markerStreamer.setRotation( marker, newData.rotation );

    if( newData.hasOwnProperty( "markerType" ) )
        markerStreamer.setMarkerType( marker, newData.markerType );

    if( newData.hasOwnProperty( "drawOnEnter" ) )
        markerStreamer.setDrawOnEnter( marker, newData.drawOnEnter );

    if( newData.hasOwnProperty( "textureName" ) )
        markerStreamer.setTextureName( marker, newData.textureName );

    if( newData.hasOwnProperty( "textureDict" ) )
        markerStreamer.setTextureDict( marker, newData.textureDict );

    if( newData.hasOwnProperty( "rotate" ) )
        markerStreamer.setRotate( marker, newData.rotate );

    if( newData.hasOwnProperty( "faceCam" ) )
        markerStreamer.setFaceCamera( marker, newData.faceCam );

    if( newData.hasOwnProperty( "bobUpDown" ) )
        markerStreamer.setBobUpDown( marker, newData.bobUpDown );

    if( newData.hasOwnProperty( "color" ) )
        markerStreamer.setColor( marker, newData.color );

    if( newData.hasOwnProperty( "scale" ) )
        markerStreamer.setScale( marker, newData.scale );

    if( newData.hasOwnProperty( "direction" ) )
        markerStreamer.setDirection( marker, newData.direction );
} );

// when a streamed in object needs to be removed
alt.onServer( "entitySync:clearCache", entityId => {
    markerStreamer.clearMarker( +entityId );
} );