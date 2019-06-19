﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Uno.AzureDevOps.Business.Extensions;
using Uno.AzureDevOps.Business.VSTS;
using Uno.AzureDevOps.Client;
using Uno.AzureDevOps.Framework.Navigation;
using Uno.AzureDevOps.Framework.Tasks;

namespace Uno.AzureDevOps.Presentation
{
	[Windows.UI.Xaml.Data.Bindable]
	public class OrganizationListPageViewModel : ViewModelBase
	{
		private readonly IStackNavigationService _navigationService;
		private readonly IVSTSRepository _vstsRepository;

		private ITaskNotifier<List<AccountData>> _organizations;

		public OrganizationListPageViewModel()
		{
			_navigationService = SimpleIoc.Default.GetInstance<IStackNavigationService>();
			_vstsRepository = SimpleIoc.Default.GetInstance<IVSTSRepository>();

			ToProjectListPage = new RelayCommand<AccountData>(account => _navigationService.ToProjectListPage(account));

			ToProfilePage = new RelayCommand(() => _navigationService.ToProfilePage());

			ToAboutPage = new RelayCommand(() => _navigationService.ToAboutPage());

			ReloadPage = new RelayCommand(() => Organizations = new TaskNotifier<List<AccountData>>(GetOrganizations()));
		}

		public ITaskNotifier<List<AccountData>> Organizations
		{
			get => _organizations;
			set => Set(() => Organizations, ref _organizations, value);
		}

		public ICommand ToProjectListPage { get; }

		public ICommand ToAboutPage { get; }

		public ICommand ToProfilePage { get; }

		public ICommand ReloadPage { get; }

		public void OnNavigatedTo()
		{
			Organizations = new TaskNotifier<List<AccountData>>(GetOrganizations());
		}

		public void NavigateToProjectListPage(AccountData account)
		{
			ToProjectListPage.Execute(account);
		}

		private async Task<List<AccountData>> GetOrganizations()
		{
			return await _vstsRepository.GetOrganizations();
		}
	}
}
