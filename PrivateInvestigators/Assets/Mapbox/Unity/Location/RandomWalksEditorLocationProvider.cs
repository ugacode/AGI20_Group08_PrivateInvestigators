namespace Mapbox.Unity.Location
{
	using System;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;

	/// <summary>
	/// The EditorLocationProvider is responsible for providing mock location and heading data
	/// for testing purposes in the Unity editor.
	/// </summary>
	public class RandomWalksEditorLocationProvider : AbstractEditorLocationProvider
	{
		/// <summary>
		/// The mock "latitude, longitude" location, respresented with a string.
		/// You can search for a place using the embedded "Search" button in the inspector.
		/// This value can be changed at runtime in the inspector.
		/// </summary>
		[SerializeField]
		[Geocode]
		string _initialLatitudeLongitude;

		/// <summary>
		/// The mock heading value.
		/// </summary>
		[SerializeField]
		[Range(0, 359)]
		float _heading;


		private Vector2d? previousLocation = null;
		private Vector2d momentum = Vector2d.zero;
		Vector2d LatitudeLongitude
		{
			get
			{
				if (previousLocation == null)
				{
					previousLocation = Conversions.StringToLatLon(_initialLatitudeLongitude);
					return previousLocation.Value;
				}
				else
				{
					var newLocation = previousLocation.Value;

					var newMomentum = UnityEngine.Random.Range(0.0f, 25.0f);
					if (newMomentum < 1.0f || momentum == Vector2d.zero)
					{
						momentum.x = UnityEngine.Random.Range(-1.0f, 1.0f);
						momentum.y = UnityEngine.Random.Range(-1.0f, 1.0f);
					}

					var randomX = 0.00001 * momentum.x * UnityEngine.Random.Range(0.2f, 1.0f);
					var randomY = 0.00001 * momentum.y * UnityEngine.Random.Range(0.2f, 1.0f);
					newLocation.x += randomX;
					newLocation.y += randomY;
					previousLocation = newLocation;
					return previousLocation.Value;
				}
			}
		}

		protected override void SetLocation()
		{
			_currentLocation.UserHeading = _heading;
			_currentLocation.LatitudeLongitude = LatitudeLongitude;
			_currentLocation.Accuracy = _accuracy;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;
		}
	}
}
