using System;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class ActionPanelView : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<ActionPanelView>
		{

		}
		public Action<ActionData> OnActionDataFilled;

		private VisualElement _container;
		private Label _numberLabel;
		private TextField _textFieldAction;
		private VisualElement _textFieldContainer;

		private IntegerField _intFieldAction;
		private VisualElement _intFieldContainer;

		private ActionData _actionData;
		private bool _isChanged = false;

		private const string FontStyleResource = "Stylesheet_GlobalFont";
		private const string USSFont = "font";
		private const string USSFontTransparent = "fontTransparent";
		private const string StyleResource = "Stylesheet_ActionPanel";
		private const string USSContainer = "container";
		private const string USSNumberLabel = "numberLabel";
		private const string USSInputField = "inputField";
		private const string USSFieldContainer = "fieldContainer";
		private const string PlaceholderText = "Попил кофе";
		private const string PlaceholderScore = "30";


		public void Initialize()
		{
			styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
			styleSheets.Add(Resources.Load<StyleSheet>(FontStyleResource));

			CreateContainer();
			CreateNumberLabel();
			CreateTextInputField();
			CreateIntInputField();
		}
		public void SetupData(ActionData actionData)
		{
			_actionData = actionData;
			_numberLabel.text = $" {actionData.EntryNumber.ToString()}:";


			SetupTextFieldsData();
			SetupIntFieldData();
		}
		private void SetupTextFieldsData()
		{

			if(_actionData.IsFirst() && _actionData.IsEmptyData())
			{
				_textFieldAction.textEdition.placeholder = PlaceholderText;
				_textFieldContainer.AddToClassList(USSFontTransparent);
			}
			else
			{
				_textFieldAction.value = _actionData.ActionName;
				_textFieldContainer.AddToClassList(USSFont);
			}
		}
		private void SetupIntFieldData()
		{
			if(_actionData.IsFirst() && _actionData.IsEmptyData())
			{
				_intFieldAction.textEdition.placeholder = PlaceholderScore;
				_intFieldContainer.AddToClassList(USSFontTransparent);
			}
			else if(_actionData.IsEmptyScore())
			{
				_intFieldContainer.AddToClassList(USSFont);
				_intFieldAction.textEdition.placeholder = "6";
				_intFieldContainer.style.display = DisplayStyle.None;
			}
			else
			{
				_intFieldAction.value = _actionData.ActionScore;
				_intFieldContainer.AddToClassList(USSFont);
			}
		}


		private void CreateContainer()
		{
			_container = new VisualElement();
			_container.AddToClassList(USSContainer);
			hierarchy.Add(_container);
		}
		private void CreateNumberLabel()
		{
			_numberLabel = new Label();
			_numberLabel.AddToClassList(USSFont);
			_numberLabel.AddToClassList(USSNumberLabel);

			_container.Add(_numberLabel);
		}

		private void CreateTextInputField()
		{
			_textFieldAction = new TextField();
			_textFieldAction.AddToClassList(USSInputField);
			_textFieldAction.labelElement.visible = false;
			_textFieldAction.multiline = true;
			_textFieldAction.textEdition.hidePlaceholderOnFocus = true;

			_textFieldContainer = _textFieldAction.Q("unity-text-input");
			_textFieldContainer.AddToClassList(USSFieldContainer);
			_textFieldAction.keyboardType = TouchScreenKeyboardType.ASCIICapable;
			_container.Add(_textFieldAction);

			_textFieldAction.RegisterCallback<FocusInEvent>(OnTextFieldFocused);
			_textFieldAction.RegisterCallback<FocusOutEvent>(OnTextFieldUnfocused);
			_textFieldAction.RegisterCallback<ChangeEvent<string>>(OnTextFieldChanged);
		}
		private void OnTextFieldFocused(FocusInEvent evt)
		{
			if(_actionData.IsFirst() && _actionData.IsEmptyActionName())
			{
				VisualElement elem = _textFieldAction.Q("unity-text-input");
				elem.AddToClassList(USSFont);
				elem.RemoveFromClassList(USSFontTransparent);
			}
		}
		private void OnTextFieldUnfocused(FocusOutEvent evt)
		{
			if(!_actionData.IsEmptyData() && _isChanged)
			{
				OnActionDataFilled?.Invoke(_actionData);
			}

			if(_actionData.IsFirst() && _actionData.IsEmptyActionName())
			{
				VisualElement elem = _textFieldAction.Q("unity-text-input");
				elem.AddToClassList(USSFontTransparent);
				elem.RemoveFromClassList(USSFont);
			}
			_isChanged = false;
		}


		private void OnTextFieldChanged(ChangeEvent<string> evt)
		{
			_actionData.ActionName = evt.newValue;
			_isChanged = true;


		}
		private void CreateIntInputField()
		{
			_intFieldAction = new IntegerField();
			_intFieldAction.AddToClassList(USSInputField);
			_intFieldAction.labelElement.visible = false;
			_textFieldAction.multiline = true;
			_intFieldAction.textEdition.hidePlaceholderOnFocus = true;
			_intFieldContainer = _intFieldAction.Q("unity-text-input");
			_intFieldContainer.AddToClassList(USSFieldContainer);

			_intFieldAction.keyboardType = TouchScreenKeyboardType.NumberPad;
			_container.Add(_intFieldAction);

			_intFieldAction.RegisterCallback<FocusInEvent>(OnIntFieldFocused);
			_intFieldAction.RegisterCallback<FocusOutEvent>(OnIntFieldUnfocused);
			_intFieldAction.RegisterCallback<ChangeEvent<int>>(OnIntFieldChanged);
		}


		private void OnIntFieldUnfocused(FocusOutEvent evt)
		{
			if(!_actionData.IsEmptyScore() && _isChanged)
			{
				OnActionDataFilled?.Invoke(_actionData);
			}
			else if((_actionData.ActionScore == Int32.MinValue || _actionData.ActionScore == 0) && !_actionData.IsFirst())
			{
				_intFieldContainer.style.display = DisplayStyle.None;
				_isChanged = false;
			}

			if(_actionData.IsFirst() && _actionData.IsEmptyScore())
			{
				VisualElement elem = _intFieldAction.Q("unity-text-input");
				elem.AddToClassList(USSFontTransparent);
				elem.RemoveFromClassList(USSFont);
			}
			_isChanged = false;

		}
		private void OnIntFieldFocused(FocusInEvent evt)
		{
			_intFieldContainer.style.display = DisplayStyle.Flex;

			if(_actionData.IsFirst() && _actionData.IsEmptyScore())
			{
				VisualElement elem = _intFieldAction.Q("unity-text-input");
				elem.AddToClassList(USSFont);
				elem.RemoveFromClassList(USSFontTransparent);
			}
			evt.StopPropagation();
		}

		private void OnIntFieldChanged(ChangeEvent<int> evt)
		{
			_actionData.ActionScore = evt.newValue;
			_isChanged = true;
		}


		public void UnregisterCallbacks()
		{
			_textFieldAction.UnregisterCallback<FocusOutEvent>(OnTextFieldUnfocused);
			_textFieldAction.UnregisterCallback<ChangeEvent<string>>(OnTextFieldChanged);

			_intFieldAction.UnregisterCallback<FocusInEvent>(OnIntFieldFocused);
			_intFieldAction.UnregisterCallback<FocusOutEvent>(OnIntFieldUnfocused);
			_intFieldAction.UnregisterCallback<ChangeEvent<int>>(OnIntFieldChanged);
		}

	}
}