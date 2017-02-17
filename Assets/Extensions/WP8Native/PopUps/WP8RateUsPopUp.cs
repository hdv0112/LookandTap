////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class WP8RateUsPopUp : WP8PopupBase {

	public Action<WP8DialogResult> OnComplete = delegate {};

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static WP8RateUsPopUp Create(string title, string message) {
		WP8RateUsPopUp popup = new GameObject("WP8RateUsPopUp").AddComponent<WP8RateUsPopUp>();
		popup.title = title;
		popup.message = message;
		
		popup.init();
		
		return popup;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		#if UNITY_WP8 || UNITY_METRO
		WP8PopUps.PopUp.ShowMessageWindow_OK_Cancel(message, title, OnOkDel, OnCancelDel);
		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	
	public void OnOkDel() {

		WP8PopUps.PopUp.ShowRateWindow();

		OnComplete(WP8DialogResult.RATED);
		dispatch(BaseEvent.COMPLETE, WP8DialogResult.RATED);
		Destroy(gameObject);
	}
	
	public void OnCancelDel() {
		OnComplete(WP8DialogResult.DECLINED);
		dispatch(BaseEvent.COMPLETE, WP8DialogResult.DECLINED);
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
