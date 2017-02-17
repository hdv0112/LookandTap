using UnityEngine;
using System;
using System.Collections;

public class UM_NotificationController : SA_Singleton<UM_NotificationController> {

	//Actions
	public Action<UM_PushRegistrationResult> OnPushIdLoaded = delegate{};
	
	
	//Events
	public const string  PUSH_ID_LOADED = "push_id_loaded";


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	public void RetrieveDevicePushId() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			GoogleCloudMessageService.instance.addEventListener(GoogleCloudMessageService.CLOUD_MESSAGE_SERVICE_REGISTRATION_FAILED, OnRegFailed);
			GoogleCloudMessageService.instance.addEventListener(GoogleCloudMessageService.CLOUD_MESSAGE_SERVICE_REGISTRATION_RECIVED, OnRegstred);
			GoogleCloudMessageService.instance.RgisterDevice();

			break;
		case RuntimePlatform.IPhonePlayer:
			#if UNITY_IPHONE
			IOSNotificationController.instance.RegisterForRemoteNotifications(RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound);
			IOSNotificationController.instance.OnDeviceTokenReceived += IOSPushTokenReceived;
			#endif
			break;
		}

	}

	public void ShowNotificationPoup(string title, string messgae) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.instance.ShowToastNotification(messgae);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.instance.ShowNotificationBanner(title, messgae);
			break;
		}
	}

	public int ScheduleLocalNotification(string title, string message, int seconds) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			return AndroidNotificationManager.instance.ScheduleLocalNotification(title, message, seconds);
		case RuntimePlatform.IPhonePlayer:
			return IOSNotificationController.instance.ScheduleNotification(seconds, message, true);
		}

		return 0;
	}

	public void CancelLocalNotification(int id) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.instance.CancelLocalNotification(id);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.instance.CancelLocalNotificationById(id);
			break;
		}
	}

	public void CancelAllLocalNotifications() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.instance.CancelAllLocalNotifications();
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.instance.CancelAllLocalNotifications();
			break;
		}
	}


	private void OnRegFailed() {
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(string.Empty, false);
		OnPushIdLoaded(result);
		dispatch(PUSH_ID_LOADED, result);
	}
	

	private void OnRegstred() {
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(GoogleCloudMessageService.instance.registrationId, true);
		OnPushIdLoaded(result);
		dispatch(PUSH_ID_LOADED, result);
	}

	private void IOSPushTokenReceived (IOSNotificationDeviceToken res){
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(res.tokenString, true);
		OnPushIdLoaded(result);
		dispatch(PUSH_ID_LOADED, result);
	}
}
