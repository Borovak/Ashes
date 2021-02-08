using System;
using System.Collections.Generic;
using System.Linq;

namespace Static
{
    public class GrassManager
    {
        private ChamberController _chamberController;
        private Dictionary<int, Dictionary<int, int>> _data;
        private int _itemCount;

        public GrassManager(ChamberController chamberController)
        {
            _chamberController = chamberController;
        }

        public void ClearGrass()
        {
            _chamberController.grass = "";
            _data = new Dictionary<int, Dictionary<int, int>>();
        }

        public void AddGrass(int x, int y, int type)
        {
            var grassStrings = GetGrassStrings();
            var grassString = string.Join(",", x, y, type);
            grassStrings.Add(grassString);
            _chamberController.grass = string.Join(";", grassStrings);
            RefreshGrassData();
        }

        public List<string> GetGrassStrings()
        {
            if (string.IsNullOrEmpty(_chamberController.grass)) return new List<string>();
            var s = _chamberController.grass.Split(';');
            return s.ToList();
        }

        public void RefreshGrassData()
        {
            _data = new Dictionary<int, Dictionary<int, int>>();
            if (string.IsNullOrEmpty(_chamberController.grass))
            {
                _itemCount = 0;
                return;
            }
            var grassStrings = GetGrassStrings();
            var count = 0;
            foreach (var grassString in grassStrings)
            {
                var s = grassString.Split(',');
                var x = Convert.ToInt32(s[0]);
                var y = Convert.ToInt32(s[1]);
                var type = Convert.ToInt32(s[2]);
                if (!_data.ContainsKey(x))
                {
                    _data.Add(x, new Dictionary<int, int>());
                }
                _data[x].Add(y, type);
                count++;
            }
            _itemCount = count;
        }

        public bool IsThereGrassThere(int x, int y)
        {
            if (_data == null)
            {
                RefreshGrassData();
            } 
            return _data.ContainsKey(x) && _data[x].ContainsKey(y);
        }

        public bool IsThereAnyGrass()
        {
            if (_data == null)
            {
                RefreshGrassData();
            } 
            return _itemCount > 0;
        }
    }
}