﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using TestAssignment.Core;

namespace TestAssignment.ViewModel
{
	public class MainViewModel: NotifyPropertyChanged
	{
		private readonly IApplicationModel _applicationModel;

		public MainViewModel(IApplicationModel applicationModel)
		{
			_applicationModel = applicationModel;

			Points = new ObservableCollection<PointViewModel>();
			Robots = applicationModel.GetRobots().Select(coordinates => new RobotViewModel(coordinates)).ToList();
			ValueRange = new Point(
				applicationModel.MaxPoint.X - applicationModel.MinPoint.X,
				applicationModel.MaxPoint.Y - applicationModel.MinPoint.Y);
		}

		public IReadOnlyList<RobotViewModel> Robots { get; }

		public ObservableCollection<PointViewModel> Points { get; }

		public Point ValueRange { get; }

		public Point MinPoint => _applicationModel.MinPoint;

		public Point MaxPoint => _applicationModel.MaxPoint;

		public double NewPointX
		{
			get => _newPointX;
			set => SetProperty(ref _newPointX, value);
		}
		private double _newPointX;

		public double NewPointY
		{
			get => _newPointY;
			set => SetProperty(ref _newPointY, value);
		}
		private double _newPointY;

		public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddPoint, CanAddPoint));

		private DelegateCommand _addCommand;

		private void AddPoint()
		{
			var newPoint = new Point(NewPointX, NewPointY);
			_applicationModel.AddPoint(newPoint);

			//It's better to do it via a model event, but for such a small assignment I'll just make it in a simple way
			Points.Add(new PointViewModel(newPoint));
			StartCommand.RaiseCanExecuteChanged();
		}

		private bool CanAddPoint()
		{
			return true;
		}

		public DelegateCommand StartCommand => _startCommand ?? (_startCommand = new DelegateCommand(Start, CanStart));
		private DelegateCommand _startCommand;

		private async void Start()
		{
			StartCommand.IsActive = true;
			StartCommand.RaiseCanExecuteChanged();

			await Task.WhenAll(GetSimulationTasks().ToArray());

			StartCommand.IsActive = false;
			StartCommand.RaiseCanExecuteChanged();
		}

		private IEnumerable<Task> GetSimulationTasks()
		{
			var paths = _applicationModel.GetPaths().ToList();
			for (var i = 0; i < paths.Count; i++)
			{
				yield return Robots[i].Simulate(paths[i], Points);
			}
		}

		private bool CanStart()
		{
			return Points.Any() && !_startCommand.IsActive;
		}
	}
}
