using System.IO;
using MvvmCross.ViewModels;

namespace ExampleBitmaps
{
    public class ExampleBitmapsViewModel : MvxViewModel
    {
        private int _iCurrentImg;
        private string _currentImg;
        private readonly string[] _images;

        public ExampleBitmapsViewModel()
        {
            _images = Directory.GetFiles("Images");
            CurrentImg = _images[0];
        }

        public string CurrentImg
        {
            get => _currentImg;
            set => SetProperty(ref _currentImg, value);
        }

        public void Next()
        {
            ++_iCurrentImg;
            if (_iCurrentImg == _images.Length)
                _iCurrentImg = 0;
            CurrentImg = _images[_iCurrentImg];
        }

        public void Prev()
        {
            --_iCurrentImg;
            if (_iCurrentImg < 0)
                _iCurrentImg = _images.Length - 1;
            CurrentImg = _images[_iCurrentImg];
        }
    }
}