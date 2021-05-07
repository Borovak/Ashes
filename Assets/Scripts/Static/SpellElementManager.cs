using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace Static
{
    public static class SpellElementManager
    {
        public static Color GetColorFromSpellElement(Constants.SpellElements spellElement)
        {
            var assoc = new Dictionary<Constants.SpellElements, Color>
            {
                {Constants.SpellElements.Fire, Color.red},
                {Constants.SpellElements.Electric, Color.yellow},
                {Constants.SpellElements.Ice, Color.cyan},
                {Constants.SpellElements.Water, Color.blue}
            };
            return assoc.TryGetValue(spellElement, out var color) ? color : Color.magenta;
        }
    }
}