using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceRecogniser
{
	class Program
	{
		// thanks Cicada
		static void Main(string[] args)
		{
			string cascadesDirPath = args[1];
			string sourcePicturePath = args[2];
			string resultPicturePath = args.ElementAtOrDefault(3) ?? "result.png";
			using(var recogniser = new FaceRecogniser(cascadesDirPath))
			{
				using(var image = Image.FromFile(sourcePicturePath))
				using(var bitmap = new Bitmap(image))
				using(var img = new Image<Bgr, byte>(bitmap))
				{
					var result = recogniser.Recognise(img);
					img.Draw(result.FacePosition, new Bgr(Color.Orange));
					img.Draw(result.EyePosition, new Bgr(Color.Blue));
					img.Draw(result.NosePosition, new Bgr(Color.Black));
					img.Draw(result.MouthPosition, new Bgr(Color.Red));
					img.Save(resultPicturePath);
				}
			}
		}
	}
}
