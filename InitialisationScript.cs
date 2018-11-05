using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using System.Threading.Tasks;

namespace Kudan.AR.Samples
{
	public class initialisationScript : MonoBehaviour {

		public KudanTracker _kudanTracker;	// The tracker to be referenced in the inspector. This is the Kudan Camera object.
		public TrackingMethodMarker _markerTracking;
		public TrackingMethodMarkerless _markerlessTracking;	// The reference to the markerless tracking method that lets the tracker know which method it is using

		//Replaced with a value from Parse
		private bool isMarkerless = true;
		public GameObject button;
		public GameObject slider;

		public GameObject findMarker;
	
		public MarkerUpdateEvent _updateMarkerEvent;
        
		public GameObject marker;
        string markerName = "";
        bool markerRetrieved = false;

		//Need a value of true or false to be passed

		// Use this for initialization
		void Start () {
				
		}
		
		// Update is called once per frame
		void Update () {
			if (markerRetrieved)
            {
                markerRetrieved = false;
                marker.GetComponent<MarkerTransformDriver>().SetExpectedID(markerName);
            }
		}


        public void SetTrackingMethod(ParseObject performance)
        {
            if (performance.Get<bool>("markerless"))
            {
                Debug.Log("Markerless");
                //if value markerless is true (-marker), then this
        //        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);    // Change the current tracking method to markerless tracking
                button.SetActive(true);
				slider.SetActive (true);

            }
            else
            {
                Debug.Log("Marker Based");
         //       _kudanTracker.ChangeTrackingMethod(_markerTracking);
                button.SetActive(false);
				slider.SetActive (false);
				findMarker.SetActive (true);
                //Also pass value here
                var query = ParseObject.GetQuery("markers")
                    .WhereEqualTo("performance", performance);
                query.FirstAsync().ContinueWith(t =>
                {
                    markerName = t.Result.Get<string>("name");
                    //Update checks for marker retrieved and when true sets the value of the marker
                    markerRetrieved = true;

                });
				Debug.Log ("MARKER NAME" + markerName);
            }
        }
	}
}
