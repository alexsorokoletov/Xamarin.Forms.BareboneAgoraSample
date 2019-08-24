using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormsBarebone
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitAgora();
        }

        private readonly IAgoraEngine _agoraCoreEngine = DependencyService.Get<IAgoraEngine>();

        private void InitAgora()
        {
            const string appId = "";
            const string token = "";
            const string channelName = "";
            _agoraCoreEngine.JoinedChannelEvent += _agoraCoreEngine_JoinedChannelEvent;
            _agoraCoreEngine.LeftChannelEvent += _agoraCoreEngine_LeftChannelEvent;
            _agoraCoreEngine.RemoteUserJoinedChannelEvent += _agoraCoreEngine_RemoteUserJoinedChannelEvent;
            _agoraCoreEngine.RemoteUserLeftChannelEvent += _agoraCoreEngine_RemoteUserLeftChannelEvent;
            _agoraCoreEngine.FirstVideoDecodedEvent += _agoraCoreEngine_FirstVideoDecodedEvent;
           _agoraCoreEngine.InitAndJoinChannel(channelName, token, appId);
        }

        private void _agoraCoreEngine_FirstVideoDecodedEvent(int uid, int elapsed)
        {
            /*
             * a remote user joined the channel (or was in the channel before we came into it)
             * we can use it's uid value to show the video from that user
             */
            Device.BeginInvokeOnMainThread(() =>
            {
                agoraCanvas.Uid = uid;
            });
        }

        private void _agoraCoreEngine_RemoteUserLeftChannelEvent(int uid, int elapsed)
        {
            /*
             * the remote user left the channel,
             * if we were showing it's video (compare uids), we should stop doing so
             */           
        }

        private void _agoraCoreEngine_RemoteUserJoinedChannelEvent(int uid, int elapsed)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                agoraCanvas.Uid = uid;
            });
        }

        

        private void _agoraCoreEngine_LeftChannelEvent()
        {
            // we disconnected from Agora and can release all resources, shut everything down etc.
        }

        private void _agoraCoreEngine_JoinedChannelEvent()
        {
            // we have Agora initialized and joined the channel
        }
    }
}
