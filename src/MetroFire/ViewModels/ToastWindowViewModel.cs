﻿using System;
using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class ToastWindowViewModel : IToastWindowViewModel
	{
		private readonly Func<IChatDocument> _chatDocumentCreator;
		private readonly IMessageBus _bus;

		public ToastWindowViewModel(Func<IChatDocument> chatDocumentCreator, IMessageBus bus)
		{
			_chatDocumentCreator = chatDocumentCreator;
			_bus = bus;

			Toasts = new ReactiveCollection<ToastViewModel>();

			bus.Listen<ShowToastMessage>().SubscribeUI(OnShowToast);

		}

		private void OnShowToast(ShowToastMessage showToastMessage)
		{

			var toast = new ToastViewModel(showToastMessage, _chatDocumentCreator(), _bus);
			Toasts.Add(toast);

			toast.Closed.SubscribeOnceUI(_ => Toasts.Remove(toast));
		}

		public ReactiveCollection<ToastViewModel> Toasts { get; private set; }
	}
}
