using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Marcidia.ComponentModel
{
    public sealed class MarcidiaComponentCollection : Collection<MarcidiaComponent>
    {
        public MarcidiaComponentCollection()
        {            
        }

        public MarcidiaComponentCollection(IList<MarcidiaComponent> list)
            : base(list)
        {
        }

        public event EventHandler<MarcidiaComponentEventArgs> ComponentAdded;

        private void OnComponentAdded(MarcidiaComponent component)
        {
            if (ComponentAdded != null)
                ComponentAdded(this, new MarcidiaComponentEventArgs(component));
        }

        public event EventHandler<MarcidiaComponentEventArgs> ComponentRemoved;

        private void OnComponentRemoved(MarcidiaComponent component)
        {
            if (ComponentRemoved != null)
                ComponentRemoved(this, new MarcidiaComponentEventArgs(component));
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
                OnComponentRemoved(item);

            base.ClearItems();
        }

        protected override void SetItem(int index, MarcidiaComponent item)
        {
            OnComponentRemoved(Items[index]);
            OnComponentAdded(item);
            
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, MarcidiaComponent item)
        {
            OnComponentAdded(item);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            OnComponentRemoved(Items[index]);

            base.RemoveItem(index);
        }
        
    }
}