using BlazorComponent;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;

namespace RadioTest.Test1
{
   public class MRadioGroup<TValue> : MInput<TValue>
    {
        [Parameter]
        public bool Column { get; set; } = true;


        [Parameter]
        public bool Mandatory { get; set; }

        [Parameter]
        public bool Row { get; set; }

        protected List<MRadio<TValue>> Items { get; set; } = new List<MRadio<TValue>>();


        protected override void SetComponentClass()
        {
            base.SetComponentClass();
            string prefix = "m-input";
            base.CssProvider.Merge(delegate (CssBuilder cssBuilder)
            {
                cssBuilder.Add(prefix + "--selection-controls").Add(prefix + "--radio-group").AddIf(prefix + "--radio-group--column", () => Column && !Row)
                    .AddIf(prefix + "--radio-group--row", () => Row);
            }).Apply("radio-group", delegate (CssBuilder cssBuilder)
            {
                cssBuilder.Add("m-input--radio-group__input");
            });
            base.AbstractProvider.Merge(typeof(BInputDefaultSlot<,>), typeof(BRadioGroupDefaultSlot<TValue>));
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetActiveRadio();
        }

        private void SetActiveRadio()
        {
            if (base.InternalValue == null)
            {
                if (Mandatory)
                {
                    MRadio<TValue> mRadio = Items.FirstOrDefault((MRadio<TValue> item) => !item.Disabled);
                    if (mRadio != null)
                    {
                        UpdateItemsState(mRadio);
                    }
                }

                return;
            }

            foreach (MRadio<TValue> item in Items)
            {
                if (EqualityComparer<TValue>.Default.Equals(item.Value, base.InternalValue))
                {
                    item.Active();
                }
                else
                {
                    item.DeActive();
                }
            }
        }

        public void AddRadio(MRadio<TValue> radio)
        {
            if (!Items.Contains(radio))
            {
                Items.Add(radio);
                radio.NotifyChange += UpdateItemsState;
            }

            SetActiveRadio();
        }

        public async Task UpdateItemsState(BRadio<TValue> radio)
        {
            foreach (MRadio<TValue> item in Items)
            {
                if (item == radio)
                {
                    item.Active();
                }
                else
                {
                    item.DeActive();
                }
            }

            base.InternalValue = radio.Value;
        }
    }
}
