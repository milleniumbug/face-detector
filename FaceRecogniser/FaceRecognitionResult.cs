using System.Drawing;

namespace FaceRecogniser
{
	class FaceRecognitionResult
	{
		public Rectangle EyePosition { get; }
		public Rectangle FacePosition { get; }
		public Rectangle MouthPosition { get; }
		public Rectangle NosePosition { get; }

		public FaceRecognitionResult(Rectangle facePosition, Rectangle eyePosition, Rectangle nosePosition,
			Rectangle mouthPosition)
		{
			EyePosition = eyePosition;
			FacePosition = facePosition;
			MouthPosition = mouthPosition;
			NosePosition = nosePosition;
		}
	}
}