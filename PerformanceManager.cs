using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;
using System.Threading.Tasks;
using UnityEngine.UI;
using Kudan.AR;
using Kudan.AR.Samples;


//namespace Kudan.AR.Samples{
	//Class Description:
	//Controls the state of the VideoManager object.
	//Includes loading/unloading , playing, pausing and stopping performances. 
	public class PerformanceManager : MonoBehaviour {

		//public KudanTracker _kudanTracker;	// The tracker to be referenced in the inspector. This is the Kudan Camera object.
		
		//Take the transform of the kudan camera
		public Transform kudan_camera;
		public Transform holder;
		MediaPlayerCtrl m_script;
	    //The VideoManager object in the scene is associated to the video object.
	    public GameObject video;
        public GameObject markerVideo;
        public GameObject markerlessVideo;
		public GameObject markerHolder;
		public SampleApp sample;
		public Text input;


	public KudanTracker _kudanTracker;

	public TrackingMethodMarker _markerTracking;
	public TrackingMethodMarkerless _markerlessTracking;	// The reference to the markerless tracking method that lets the tracker know which method it is using


		public MediaPlayerEvent mediaevent;
		//Control the destroy/instantiate VM
		GameObject clone = null;
		bool loadingbool = false;

		GameObject marker_clone = null;
		private bool isMarkerless = true;
		
		GameObject clone_image = null;
		public GameObject loading;
		public GameObject loadingImage;
		public GameObject findMarker;

		//Add VM to slider
		SliderControl sliderControl;
		public GameObject mainSlider;
		
	    //Used in the Update method to dtermine when to start loading a new performance
	    bool videoLoading = false;

	    //The performance object that is currently in use.
	    ParseObject currentPerformance = null;

	    //The text representing the artist currently performing.
	    public Text artistName;

	    public GameObject sampleUI;
	    initialisationScript arInitScript;

	    public GameObject playPause;

		void Start () {
	        arInitScript = sampleUI.GetComponent<initialisationScript>();
			sliderControl = mainSlider.GetComponent<SliderControl> ();
			sample = sampleUI.GetComponent<SampleApp> ();

	
		} 
		
		// Update is called once per frame
		void Update () {


		
	        //If a new perofmance is loading, check every frame to see if it is ready to be played.
	        //Once it is ready, stop the performance so that it's ready to begin when play is selected.
			if (videoLoading)
	        {
	            if (video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY)
	            {
	                StopPerformance();
	                videoLoading = false;
	            }
	            //Debug.Log(video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	        }
		}

		public void lookAtCamera(){
			holder.transform.LookAt (kudan_camera);	
		}

	    public void LoadPerformance(ParseObject performance)
	    {
        	if (performance.Get<bool>("markerless"))
            {
      		    video = markerlessVideo;
				isMarkerless = true;
            }
            else
            {
				Destroy (marker_clone);

				marker_clone = Instantiate (markerVideo, markerHolder.transform);
				video = markerVideo;
				mediaevent.passClone (marker_clone);
				isMarkerless = false;

			//video.GetComponent<MediaPlayerCtrl> ().resetID ("Heino");
            }
			
	        currentPerformance = performance;
	        Debug.Log(performance.Get<string>("performanceFile"));
	        video.GetComponent<MediaPlayerCtrl>().Load(performance.Get<string>("performanceFile"));

			//Get marker for marker tracking
		//	marker_ID.GetComponent<MarkerTransformDriver>().SetExpectedID(performance.Get<string>("name"));	

	        videoLoading = true;
	        Debug.Log("Video State: " + video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	        arInitScript.SetTrackingMethod(performance);

			//taken out when getting rid of pause button
	       // playPause.GetComponent<PlayPauseScript>().Reset();

		}

        public void LoadPerformance(string performance)
        {
            video.GetComponent<MediaPlayerCtrl>().Load(performance);
        }

	    public void StopPerformance()
	    {
	        video.GetComponent<MediaPlayerCtrl>().Stop();
	        Debug.Log("Video State: " + video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	    }

	    public void PausePerformance()
	    {
		Debug.Log ("Pause worked");
		    video.GetComponent<MediaPlayerCtrl>().Pause();
		    Debug.Log("Video State: " + video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	    }

	    //Method Description:
	    //Play the performance if it is ready to be played. The ready states are PAUSED, STOPPED, and READY.
	    public void PlayPerformance()
		{
			Destroy (clone);
			input.text = "Play Button";
			clone = Instantiate (video, holder.transform);
			clone.transform.localScale += new Vector3 (-299, -299, -299);
			if (loadingbool == false) {
				loadingImage.SetActive (true);
				loadingbool = true;
			} else {
				loadingbool = false;
			}


			//	clone_image.gameObject.transform.localScale += new Vector3 (500.0F, 500.0F, 500.0F);
			mediaevent.passClone (clone);
			//	Instantiate (icon, holder.transform);
				
			sliderControl.setSliderScript (clone);
		//	clone.GetComponent<PlayPauseScript> ().setarUI ();
				//realign when button is pressed
			holder.transform.LookAt (kudan_camera);	
			
			Debug.Log ("Tracking: ");
	        Debug.Log(video.GetComponent<MediaPlayerCtrl>().GetCurrentState() + ", " + video.GetComponent<MediaPlayerCtrl>().GetCurrentSeekPercent());
	        if (video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY
	            || video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED
	            || video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
	        {
	            video.GetComponent<MediaPlayerCtrl>().Play();
	        }
			Debug.Log("Video State: " + video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	    }
		

		public void test(){
			Debug.Log ("Get here?");
		}

		public void setActiveVM(){
			clone.SetActive (false);
		}

		public void SampleRemoveMarker(){
			sample.RemoveMarker ();
		}

		public void destroyImageClone(){
		//	Destroy (clone_image);
			loadingImage.SetActive(false);
		}

		public void destroyClone(){
			Destroy(clone);
		}

		public void destroyMarkerClone(){
			Destroy (marker_clone);
		}

		public void ResizeClone(){
			
			if (isMarkerless) {
				clone.transform.localScale += new Vector3 (299, 299, 299);

			} else {
				Debug.Log ("Cool");
			}
		}

		public void loadingBoolFalse(){
			loadingbool = false;
		}

	    public void UnloadPerformance()
	    {
			input.text = "Unload Button";
	        video.GetComponent<MediaPlayerCtrl>().UnLoad();
	        Debug.Log("Video State: " + video.GetComponent<MediaPlayerCtrl>().GetCurrentState());
	    }


	    //Method Description:
	    //Method is called by the Play/Pause button. Toggles the state of the video between playing and paused.
	    public void PlayPause()
	    {

	        if (video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
	        {
				Debug.Log("PAUSE");
	            PausePerformance();
	        }
	        else if(video.GetComponent<MediaPlayerCtrl>().GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
	        {
			Debug.Log ("PLAY");
	            PlayPerformance();
	        }
	    }

	    public void LoadArtist(ParseObject artist)
	    {
            artistName.text = "";
	        //artistName.text = artist.Get<string>("name");
	    }

	    public ParseObject GetPerformance()
	    {
	        return currentPerformance;
	    }

    public MediaPlayerCtrl.MEDIAPLAYER_STATE GetState()
    {
        return video.GetComponent<MediaPlayerCtrl>().GetCurrentState();
    }
	}
//}
