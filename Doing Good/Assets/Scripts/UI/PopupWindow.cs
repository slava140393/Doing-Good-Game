using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class PopupWindow : VisualElement
	{
		private VisualElement _popupWindow;
		public new class UxmlFactory : UxmlFactory<PopupWindow>
		{

		}

		private const string LabelText = "\tВсё справедливо. Всё зависит от состояния! От богатства, т.е. от добра.\n" +
		                                 "Богатство от слова Бог. Божественность, вот что важно!\n" +
		                                 "Чем лучше состояние, тем больше возможностей делать добра ещё больше! И богатеть ещё! " +
		                                 "В плохом состоянии вообще не возможно делать никакого добра, " +
		                                 "в плохом состоянии происходит только ухудшение и вред. Если плохое состояние," +
		                                 " важно срочно учиться не делать никакого зла! 100% не делать никакого зла. " +
		                                 " Выбрать правильный курс в жизни, к богатству! " +
		                                 "\tВсё зависит от состояния души, игра “Делать добро” - для души. " +
		                                 "Какое состояние души, такая и жизнь у человека.\n" +
		                                 "\tСостояния души:\n" +
		                                 "Божественность\t - 100% здоровья\n" +
		                                 "Прославленность\t - 90% здоровья\n" +
		                                 "Благородность\t - 80% здоровья\n" +
		                                 "Уважаемость\t - 70% здоровья\n" +
		                                 "Добродетельность\t - 60% здоровья\n" +
		                                 "Нейтральность\t - 50% здоровья\n" +
		                                 "Вредоносность\t - 40% здоровья\n" +
		                                 "Презрительность\t - 30% здоровья\n" +
		                                 "Отвратительность\t - 20% здоровья\n" +
		                                 "Умалишенность\t - 10% здоровья\n" +
		                                 "Микромобность\t - 0% здоровья\n\n" +
		                                 "\tСпросите себя какое у вас самочувствие сейчас? На сколько % из 100. " +
		                                 "И заметьте, как улучшается ваше состояние, когда вы делаете добро.\n" +
		                                 "\tУдачи как можно скорее разбогатеть!";

		private const string StyleResource = "Stylesheet_PopupWindow";
		private const string StyleResourceFont = "Stylesheet_GlobalFont";
		private const string USSPopupWindowElem = "popup";
		private const string USSPopupWindowLabel = "popup_text";
		private const string USSWindow = "window";
		private const string USSFont = "font";

		public PopupWindow()
		{
			styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
			styleSheets.Add(Resources.Load<StyleSheet>(StyleResourceFont));
			this.AddToClassList(USSWindow);

			SetupPopupWindowElem();
			SetupLabel();
			Hide(null);
		}
		private void SetupPopupWindowElem()
		{
			_popupWindow = new VisualElement();
			hierarchy.Add(_popupWindow);
			_popupWindow.AddToClassList(USSPopupWindowElem);
			_popupWindow.RegisterCallback<PointerDownEvent>(Hide);
		}
		private void SetupLabel()
		{
			Label label = new Label();
			label.AddToClassList(USSFont);
			label.AddToClassList(USSPopupWindowLabel);
			label.text = LabelText;
			_popupWindow.Add(label);
		}

		public void Show()
		{
			this.style.visibility = Visibility.Visible;
		}
		private void Hide(PointerDownEvent evt)
		{
			this.style.visibility = Visibility.Hidden;
		}

	}
}