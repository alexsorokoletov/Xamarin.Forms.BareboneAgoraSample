using System;
using Android.Content;
using FormsBarebone;
using FormsBarebone.Droid;
using Xamarin.Forms;
using DT.Xamarin.Agora;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Views;
using DT.Xamarin.Agora.Video;

[assembly: ExportRenderer(typeof(AgoraVideoCanvas), typeof(AgoraVideoCanvasRenderer))]
namespace FormsBarebone.Droid
{
    public class AgoraVideoCanvasRenderer : ViewRenderer<AgoraVideoCanvas, FrameLayout>
    {

        public AgoraVideoCanvasRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AgoraVideoCanvas> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null) // suggested read - the renderer implementation guide from Xamarin https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view
            {
                if (Control == null)
                {
                    var view = (FrameLayout)LayoutInflater.From(this.Context).Inflate(Resource.Layout.AgoraVideoLayout, null, false);
                    SetNativeControl(view);
                    InitializeAgoraAndroidControl(view, e.NewElement.Uid);
                }
                else
                {
                    InitializeAgoraAndroidControl(Control, e.NewElement.Uid);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(AgoraVideoCanvas.Uid))
            {
                if (Control != null)
                {
                    var uid = Element.Uid;
                    InitializeAgoraAndroidControl(Control, Element.Uid);
                }
            }
        }

        private void InitializeAgoraAndroidControl(FrameLayout container, int uid)
        {
            Console.WriteLine($"Renderer - we should show stream with uid = {uid}");
            if (uid == 0)
            {
                //stop video
            }
            else
            {
                if (container.ChildCount == 0)
                {
                    var surfaceView = RtcEngine.CreateRendererView(DemoHelper.Context);
                    container.AddView(surfaceView);
                    var videoCanvas = new VideoCanvas(surfaceView, VideoCanvas.RenderModeHidden, uid);
                    var rtcEngine = DemoHelper.CurrentAgoraEngine.RtcEngine;
                    var result = rtcEngine.SetupRemoteVideo(videoCanvas);
                    Console.WriteLine($"Set remote video result {result}");
                }
            }
        }

        /*
         * see https://docs.agora.io/en/Video/publish_android
        private void setupRemoteVideo(int uid) {
           FrameLayout container = (FrameLayout) findViewById(R.id.remote_video_view_container);

           if (container.getChildCount() >= 1) {
               return;
           }

           SurfaceView surfaceView = RtcEngine.CreateRendererView(getBaseContext());
           container.addView(surfaceView);
           mRtcEngine.setupRemoteVideo(new VideoCanvas(surfaceView, VideoCanvas.RENDER_MODE_ADAPTIVE, uid));

           surfaceView.setTag(uid);
           View tipMsg = findViewById(R.id.quick_tips_when_use_agora_sdk);
           tipMsg.setVisibility(View.GONE);
          }

        */
    }
}
