﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestAssignment.ViewModel;

namespace TestAssignment
{
	public class Robot : ContentControl
	{
		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
			nameof(Target), typeof(StepViewModel), typeof(Robot), new PropertyMetadata(null, TargetChanged));

		public StepViewModel Target
		{
			get => (StepViewModel)GetValue(TargetProperty);
			set => SetValue(TargetProperty, value);
		}

		public static readonly RoutedEvent MovingEvent = EventManager.RegisterRoutedEvent(
			nameof(Moving), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Robot));

		public event RoutedEventHandler Moving
		{
			add => AddHandler(MovingEvent, value);
			remove => RemoveHandler(MovingEvent, value);
		}

		private void RaiseMovingEvent()
		{
			var newEventArgs = new RoutedEventArgs(MovingEvent);
			RaiseEvent(newEventArgs);
		}

		private static void TargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(((StepViewModel)e.NewValue).Duration == default(TimeSpan))
				return;

			((Robot)d).RaiseMovingEvent();
		}
	}
}