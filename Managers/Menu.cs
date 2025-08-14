using Kerl0s_ModMenu.Utils.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kerl0s_ModMenu.Managers
{
    // Classe basique pour un Menu
    public class Menu
    {
        public string Name;
        public Color HeaderColor;
        public List<string> Items;
        public List<Action> Actions;
        public List<bool> Toggles = new List<bool>();  // Pour stocker les états ON/OFF

        public int SelectedIndex = 0;

        public Menu(string name, Color headerColor, IEnumerable<string> items, IEnumerable<Action> actions = null)
        {
            Name = name;
            HeaderColor = headerColor;
            Items = new List<string>(items);

            if (actions == null)
            {
                // Pas d'actions, on crée une liste vide d’actions
                Actions = new List<Action>(new Action[Items.Count]);
                for (int i = 0; i < Actions.Count; i++)
                    Actions[i] = () => { }; // actions vides par défaut
            }
            else
            {
                Actions = new List<Action>(actions);

                // Si moins d'actions que d'items, on complète avec des actions vides
                while (Actions.Count < Items.Count)
                    Actions.Add(() => { });

                // Si plus d'actions que d'items, on coupe la liste d’actions
                if (Actions.Count > Items.Count)
                    Actions.RemoveRange(Items.Count, Actions.Count - Items.Count);
            }
        }

        public void Draw()
        {
            if (!MenuManager.IsOpen || MenuManager.CurrentMenu != this) return;

            float startX = 0.11f;
            float startY = 0.09f;
            float spacing = 0.05f;

            UIDrawer.DrawHeader($"KMM V2 - {Name}", 1, .6f, HeaderColor, 255, startX, startY - 0.05f);

            for (int i = 0; i < Items.Count; i++)
            {
                var color = (i == SelectedIndex) ? Color.Gray : Color.Black;
                UIDrawer.DrawHeader(Items[i], 4, 0.5f, color, 100, startX, startY + (i * spacing));
            }
        }

        public void SelectNext()
        {
            SelectedIndex = (SelectedIndex + 1) % Items.Count;
        }

        public void SelectPrevious()
        {
            SelectedIndex = (SelectedIndex - 1 + Items.Count) % Items.Count;
        }

        public void ActivateSelected()
        {
            if (SelectedIndex < Toggles.Count)
            {
                // Inverse toggle si possible
                Toggles[SelectedIndex] = !Toggles[SelectedIndex];
                Actions[SelectedIndex]?.Invoke();
            }
            else
            {
                Actions[SelectedIndex]?.Invoke();
            }
        }
    }
}
