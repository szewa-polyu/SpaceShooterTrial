// https://github.com/elraccoone/react-unity-webgl/wiki/Communication-from-Unity-to-React
mergeInto(LibraryManager.library, {

  // Create a new function with the same name as
  // the event listeners name and make sure the
  // parameters match as well.

  SendPlayerPositionAsString: function(position) {

    // Within the function we're going to trigger
    // the event within the ReactUnityWebGL object
    // which is exposed by the library to the window.

	// https://forum.unity.com/threads/pointer_stringify-is-returning-garbled-text.481419/
	// Pointer_stringify
    ReactUnityWebGL.PlayerPositionAsStringArrived(Pointer_stringify(position));
  },
  
  SendPlayerPositionAsVector: function(position) {

    // Within the function we're going to trigger
    // the event within the ReactUnityWebGL object
    // which is exposed by the library to the window.

    ReactUnityWebGL.PlayerPositionAsVectorArrived(position);
  }
});