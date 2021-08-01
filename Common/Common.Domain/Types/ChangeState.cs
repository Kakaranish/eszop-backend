namespace Common.Domain.Types
{
    public class ChangeState<T>
    {
        private readonly bool _nullable;
        
        public T OldValue { get; }
        public T NewValue { get; }

        public bool Changed => (_nullable || NewValue != null) && 
                               ((OldValue == null && !ReferenceEquals(OldValue, NewValue)) || !OldValue.Equals(NewValue));

        public ChangeState(T oldValue, T newValue, bool nullable = false)
        {
            OldValue = oldValue;
            NewValue = newValue;
            
            _nullable = nullable;
        }
    }
}
