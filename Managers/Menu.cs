using Kerl0s_ModMenu.Utils.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kerl0s_ModMenu.Managers
{
    // Classe basique pour un Menu
    public class Menu
    {
        public string Name { get; set; }
        public Color HeaderColor { get; set; }
        public List<string> Items { get; set; }
        public List<Action> Actions { get; set; }
        public List<bool> Toggles { get; set; } = new List<bool>();

        public int SelectedIndex { get; set; } = 0;

        public Menu(string name, Color headerColor, IEnumerable<string> items, IEnumerable<Action> actions = null)
        {
            Name = name;
            HeaderColor = headerColor;
            Items = new List<string>(items);

            if (actions == null)
            {
                Actions = new List<Action>(new Action[Items.Count]);
                for (int i = 0; i < Actions.Count; i++)
                    Actions[i] = () => { };
            }
            else
            {
                Actions = new List<Action>(actions);

                while (Actions.Count < Items.Count)
                    Actions.Add(() => { });

                if (Actions.Count > Items.Count)
                    Actions.RemoveRange(Items.Count, Actions.Count - Items.Count);
            }

            // Ensure Toggles list size aligns with items (defaults to false)
            while (Toggles.Count < Items.Count) Toggles.Add(false);
            if (Toggles.Count > Items.Count) Toggles.RemoveRange(Items.Count, Toggles.Count - Items.Count);
        }

        public void Draw()
        {
            if (!MenuManager.IsOpen || MenuManager.CurrentMenu != this) return;
            if (Items == null || Items.Count == 0) return;

            const float startX = 0.11f;
            const float startY = 0.09f;
            const float spacing = 0.05f;

            UIDrawer.DrawHeader($"KMM V2 - {Name}", 1, .6f, HeaderColor, 255, startX, startY - 0.05f);

            for (int i = 0; i < Items.Count; i++)
            {
                var color = (i == SelectedIndex) ? Color.Gray : Color.Black;
                UIDrawer.DrawHeader(Items[i], 4, 0.5f, color, 100, startX, startY + (i * spacing));
            }
        }

        public void SelectNext()
        {
            if (Items == null || Items.Count == 0) return;
            SelectedIndex = (SelectedIndex + 1) % Items.Count;
        }

        public void SelectPrevious()
        {
            if (Items == null || Items.Count == 0) return;
            SelectedIndex = (SelectedIndex - 1 + Items.Count) % Items.Count;
        }

        public void ActivateSelected()
        {
            if (Items == null || Items.Count == 0) return;
            if (SelectedIndex < 0 || SelectedIndex >= Items.Count) return;

            // Toggle if there's a corresponding toggle state
            if (SelectedIndex < Toggles.Count)
            {
                Toggles[SelectedIndex] = !Toggles[SelectedIndex];
            }

            Actions?[SelectedIndex]?.Invoke();
        }
    }
}
