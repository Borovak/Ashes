using Classes;
using UnityEngine;

namespace Interfaces
{
    public interface IIconElement
    {
        Sprite sprite { get; }
        int id { get; }
        string name { get; }
        int quantity { get; }
        Constants.IconElementTypes iconElementType { get; }
    }
}