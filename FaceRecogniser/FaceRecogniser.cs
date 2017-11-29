using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceRecogniser
{
	class FaceRecogniser : IDisposable
	{
		private readonly CascadeClassifier faceClassifier;
		private readonly CascadeClassifier mouthClassifier;
		private readonly CascadeClassifier noseClassifier;
		private readonly CascadeClassifier eyesClassifier;

		private static Rectangle Offset(Rectangle rect, Point location)
		{
			rect.Offset(location);
			return rect;
		}

		public FaceRecognitionResult Recognise(Image<Bgr, byte> img)
		{
			foreach(var faceRectangle in faceClassifier.DetectMultiScale(img))
			{
				var faceImg = img.GetSubRect(faceRectangle);
				foreach(var eyesRectangle in eyesClassifier.DetectMultiScale(faceImg)
					.Select(rect => Offset(rect, faceRectangle.Location)))
				{
					foreach(var noseRectangle in noseClassifier.DetectMultiScale(faceImg)
						.Select(rect => Offset(rect, faceRectangle.Location)))
					{
						foreach(var mouthRectangle in mouthClassifier.DetectMultiScale(faceImg)
							.Select(rect => Offset(rect, faceRectangle.Location)))
						{
							Func<bool> sanityCheck = () =>
								eyesRectangle.Top < noseRectangle.Top &&
								eyesRectangle.Bottom < noseRectangle.Bottom &&
								noseRectangle.Top < mouthRectangle.Top &&
								noseRectangle.Bottom < mouthRectangle.Bottom;
							if(sanityCheck())
							{
								return new FaceRecognitionResult(
									faceRectangle,
									eyesRectangle,
									noseRectangle,
									mouthRectangle);
							}
						}
					}
				}
			}
			throw new Exception("no non-overlapping facial features");
		}

		public FaceRecogniser(string classifierDirectory)
		{
			try
			{
				faceClassifier = new CascadeClassifier(Path.Combine(classifierDirectory, "face.xml"));
				mouthClassifier = new CascadeClassifier(Path.Combine(classifierDirectory, "mouth.xml"));
				noseClassifier = new CascadeClassifier(Path.Combine(classifierDirectory, "nose.xml"));
				eyesClassifier = new CascadeClassifier(Path.Combine(classifierDirectory, "eyes.xml"));
			}
			catch(Exception)
			{
				Dispose();
				throw;
			}
		}

		public void Dispose()
		{
			faceClassifier?.Dispose();
			mouthClassifier?.Dispose();
			noseClassifier?.Dispose();
			eyesClassifier?.Dispose();
		}
	}
}