namespace Planetoid_DB
{
	/// <summary>
	/// This partial class contains methods for calculating various orbital elements
	/// and properties of ellipses, such as the semi-minor axis, linear eccentricity,
	/// major and minor axes, eccentric anomaly, true anomaly, perihelion and aphelion
	/// distances, longitude of the descending node, argument of aphelion, focal parameter,
	/// semi-latus rectum, orbital period, orbital area, orbital perimeter, semi-mean axis,
	/// mean axis, and the standard gravitational parameter.
	/// </summary>
	public partial class PlanetoidDbForm
	{
		/// <summary>
		/// Calculates the semi-minor axis of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The semi-minor axis of the ellipse.</returns>
		private static double CalculateSemiMinorAxis(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * Math.Sqrt(d: 1 - Math.Pow(x: numericalEccentricity, y: 2));

		/// <summary>
		/// Calculates the linear eccentricity of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The linear eccentricity of the ellipse.</returns>
		private static double CalculateLinearEccentricity(double semiMajorAxis, double numericalEccentricity)
		{
			double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
			return numericalEccentricity switch
			{
				0 => 0,
				< 1 and > 0 => Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) - Math.Pow(x: semiMinorAxis, y: 2)),
				> 1 => Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + Math.Pow(x: semiMinorAxis, y: 2)),
				_ => 0
			};
		}

		/// <summary>
		/// Calculates the major axis of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <returns>The major axis of the ellipse.</returns>
		private static double CalculateMajorAxis(double semiMajorAxis) => 2 * semiMajorAxis;

		/// <summary>
		/// Calculates the minor axis of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The minor axis of the ellipse.</returns>
		private static double CalculateMinorAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// Calculates the eccentric anomaly of an orbit.
		/// </summary>
		/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
		/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
		/// <returns>The eccentric anomaly of the orbit.</returns>
		private static double CalculateEccentricAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
		{
			const double k = Math.PI / 180.0;
			const int maxIteration = 30;
			int i = 0;
			double delta = Math.Pow(x: 10, y: -numberDecimalPlaces);
			meanAnomaly /= 360.0;
			meanAnomaly = 2.0 * Math.PI * (meanAnomaly - Math.Floor(d: meanAnomaly));
			double e = numericalEccentricity < 0.8 ? meanAnomaly : Math.PI;
			double f = e - (numericalEccentricity * Math.Sin(a: meanAnomaly)) - meanAnomaly;
			while ((Math.Abs(value: f) > delta) && (i < maxIteration))
			{
				e -= f / (1.0 - (numericalEccentricity * Math.Cos(d: e)));
				f = e - (numericalEccentricity * Math.Sin(a: e)) - meanAnomaly;
				i += 1;
			}
			e /= k;
			return Math.Round(a: e * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
		}

		/// <summary>
		/// Calculates the true anomaly of an orbit.
		/// </summary>
		/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
		/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
		/// <returns>The true anomaly of the orbit.</returns>
		private static double CalculateTrueAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
		{
			const double k = Math.PI / 180.0;
			double e = CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: numberDecimalPlaces);
			double s = Math.Sin(a: e);
			double c = Math.Cos(d: e);
			double fak = Math.Sqrt(d: 1.0 - (numericalEccentricity * numericalEccentricity));
			double phi = Math.Atan2(y: fak * s, x: c - numericalEccentricity) / k;
			return Math.Round(a: phi * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
		}

		/// <summary>
		/// Calculates the perihelion distance of an orbit.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
		/// <returns>The perihelion distance of the orbit.</returns>
		private static double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 - numericalEccentricity) * semiMajorAxis;

		/// <summary>
		/// Calculates the aphelion distance of an orbit.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
		/// <returns>The aphelion distance of the orbit.</returns>
		private static double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 + numericalEccentricity) * semiMajorAxis;

		/// <summary>
		/// Calculates the longitude of the descending node of an orbit.
		/// </summary>
		/// <param name="longitudeAscendingNode">The longitude of the ascending node of the orbit.</param>
		/// <returns>The longitude of the descending node of the orbit.</returns>
		private static double CalculateLongitudeDescendingNode(double longitudeAscendingNode) =>
			longitudeAscendingNode switch
			{
				>= 0 and < 180 => longitudeAscendingNode + 180,
				>= 180 and < 360 => longitudeAscendingNode - 180,
				_ => -1
			};

		/// <summary>
		/// Calculates the argument of aphelion of an orbit.
		/// </summary>
		/// <param name="argumentAphelion">The argument of perihelion of the orbit.</param>
		/// <returns>The argument of aphelion of the orbit.</returns>
		private static double CalculateArgumentOfAphelion(double argumentAphelion) =>
			argumentAphelion switch
			{
				>= 0 and < 180 => argumentAphelion + 180,
				>= 180 and < 360 => argumentAphelion - 180,
				_ => -1
			};

		/// <summary>
		/// Calculates the focal parameter of an orbit.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
		/// <returns>The focal parameter of the orbit.</returns>
		private static double CalculateFocalParameter(double semiMajorAxis, double numericalEccentricity)
		{
			double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
			return numericalEccentricity switch
			{
				> 1 => Math.Pow(x: semiMinorAxis, y: 2) / Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + Math.Pow(x: semiMinorAxis, y: 2)),
				> 0 and < 1 => Math.Pow(x: semiMinorAxis, y: 2) / Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) - Math.Pow(x: semiMinorAxis, y: 2)),
				_ => 2 * semiMajorAxis
			};
		}

		/// <summary>
		/// Calculates the semi-latus rectum of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The semi-latus rectum of the ellipse.</returns>
		private static double CalculateSemiLatusRectum(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 - Math.Pow(x: numericalEccentricity, y: 2));

		/// <summary>
		/// Calculates the latus rectum of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The latus rectum of the ellipse.</returns>
		private static double CalculateLatusRectum(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// Calculates the orbital period of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <returns>The orbital period of the ellipse.</returns>
		private static double CalculatePeriod(double semiMajorAxis) => Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3));

		/// <summary>
		/// Calculates the orbital area of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The orbital area of the ellipse.</returns>
		private static double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity)
		{
			double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
			double term1 = semiMajorAxis + semiMinorAxis;
			double term2 = 3 * Math.Pow(x: semiMajorAxis - semiMinorAxis, y: 2) / (10 * term1);
			double term3 = Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + (14 * semiMajorAxis * semiMinorAxis) + Math.Pow(x: semiMinorAxis, y: 2));
			return term1 + term2 + term3;
		}

		/// <summary>
		/// Calculates the orbital perimeter of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The orbital perimeter of the ellipse.</returns>
		private static double CalculateOrbitalPerimeter(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) * Math.PI;

		/// <summary>
		/// Calculates the semi-mean axis of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The semi-mean axis of the ellipse.</returns>
		private static double CalculateSemiMeanAxis(double semiMajorAxis, double numericalEccentricity) => (semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity)) / 2;

		/// <summary>
		/// Calculates the mean axis of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
		/// <returns>The mean axis of the ellipse.</returns>
		private static double CalculateMeanAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// Calculates the standard gravitational parameter of an ellipse.
		/// </summary>
		/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
		/// <returns>The standard gravitational parameter of the ellipse.</returns>
		private static double CalculateStandardGravitationalParameter(double semiMajorAxis) => 4 * Math.Pow(x: Math.PI, y: 2) * Math.Pow(x: semiMajorAxis, y: 3) / CalculatePeriod(semiMajorAxis: semiMajorAxis);
	}
}