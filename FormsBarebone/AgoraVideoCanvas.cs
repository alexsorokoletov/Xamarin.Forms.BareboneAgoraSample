using Xamarin.Forms;

namespace FormsBarebone
{
    public class AgoraVideoCanvas : View
    {
        public static readonly BindableProperty UidProperty = BindableProperty.Create(nameof(Uid),typeof(int),typeof(AgoraVideoCanvas), (int)0);

        public int Uid
        {
            get { return (int)GetValue(UidProperty); }
            set { SetValue(UidProperty, value); }
        }
    }

}
