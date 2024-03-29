﻿using System;

namespace TestAssignment.ViewModel
{
	public class StepViewModel
	{
		public static readonly double SpeedFactor = 0.02;

		public StepViewModel(PointViewModel destination, double distance)
		{
			Destination = destination;
			Duration = TimeSpan.FromSeconds(distance * SpeedFactor);
		}

		public PointViewModel Destination { get; }

		public TimeSpan Duration { get; }
	}
}
