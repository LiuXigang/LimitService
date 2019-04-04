using System;

namespace LimitService
{
    //限流组件,采用数组做为一个环
    public class LimitService
    {
        //当前指针的位置
        private int _currentIndex = 0;
        //限制的时间的秒数，即：x秒允许多少请求
        private readonly int _limitTimeSencond = 1;
        //请求环的容器数组
        private DateTime?[] _requestRing = null;
        //容器改变或者移动指针时候的锁
        private readonly object _objLock = new object();
        public LimitService(int countPerSecond, int limitTimeSencond)
        {
            _requestRing = new DateTime?[countPerSecond];
            this._limitTimeSencond = limitTimeSencond;
        }
        //程序是否可以继续
        public bool IsContinue()
        {
            lock (_objLock)
            {
                var currentNode = _requestRing[_currentIndex];
                //如果当前节点的值加上设置的秒 超过当前时间，说明超过限制
                if (currentNode != null && currentNode.Value.AddSeconds(_limitTimeSencond) > DateTime.Now)
                {
                    return false;
                }
                //当前节点设置为当前时间
                _requestRing[_currentIndex] = DateTime.Now;
                //指针移动一个位置
                MoveNextIndex(ref _currentIndex);
            }
            return true;
        }
        //改变每秒可以通过的请求数
        public bool ChangeCountPerSecond(int countPerSecond)
        {
            lock (_objLock)
            {
                _requestRing = new DateTime?[countPerSecond];
                _currentIndex = 0;
            }
            return true;
        }
        //指针往前移动一个位置
        private void MoveNextIndex(ref int currentIndex)
        {
            if (currentIndex != _requestRing.Length - 1)
            {
                currentIndex = currentIndex + 1;
            }
            else
            {
                currentIndex = 0;
            }
        }
    }
}
