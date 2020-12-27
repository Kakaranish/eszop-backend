namespace Common.Types
{
    public class ChangeState<T>
    {
        private readonly bool _nullable;
        
        public T OldValue { get; }
        public T NewValue { get; }

        public bool Changed => (_nullable || NewValue is not null) && !OldValue.Equals(NewValue);

        public ChangeState(T oldValue, T newValue, bool nullable = false)
        {
            OldValue = oldValue;
            NewValue = newValue;
            
            _nullable = nullable;
        }
    }
}
