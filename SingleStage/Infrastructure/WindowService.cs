//using SingleStage.DAC;
//using SingleStage.ViewModels;
//using SingleStage.Windows;
//using System.Runtime.Serialization.DataContracts;

//namespace SingleStage.Infrastructure
//{
//    public class WindowService
//    {
//        private readonly ArtistDAC _artistDAC;

//        public WindowService(ArtistDAC artistDAC)
//        {
//            ArgumentNullException.ThrowIfNull(artistDAC);

//            _artistDAC = artistDAC;
//        }

//        public void ShowManageArtists()
//        {
//            ManageArtistsViewModel manageArtistsViewModel =
//                new ManageArtistsViewModel(_artistDAC);

//            ManageArtistsWindow window =
//                new ManageArtistsWindow
//                {
//                    DataContext = manageArtistsViewModel
//                };

//            window.ShowDialog();
//        }
//    }
//}
