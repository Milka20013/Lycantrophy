namespace Lycanthropy.Inventory
{
    public class EssenceInventory : Inventory
    {
        private Player player;

        private void Awake()
        {
            player = GetComponentInParent<Player>();
        }

        protected override void RegisterItemStack(ItemStack itemStack)
        {
            base.RegisterItemStack(itemStack);
            EquipItem(itemStack);
        }

        protected override void UnRegisterItemStack(ItemStack itemStack)
        {
            base.UnRegisterItemStack(itemStack);
            UnequipItem(itemStack);
        }

        public void UnequipItem(ItemStack itemStack)
        {
            var essence = itemStack.itemUI.GetComponent<Essence>();
            UnRegisterAmplifiers(essence.essenceBlueprint.amplifiers);
            UnRegisterAmplifiers(essence.amplifiers.ToArray());
        }

        public void EquipItem(ItemStack itemStack)
        {
            var essence = itemStack.itemUI.GetComponent<Essence>();
            RegisterAmplifiers(essence.essenceBlueprint.amplifiers);
            RegisterAmplifiers(essence.amplifiers.ToArray());
        }

        private void RegisterAmplifiers(Amplifier[] amplifiers)
        {
            player.playerStats.RegisterAmplifiers(amplifiers);
        }

        private void UnRegisterAmplifiers(Amplifier[] amplifiers)
        {
            player.playerStats.UnRegisterAmplifiers(amplifiers);
        }

        public override T GetOwner<T>() where T : class
        {
            if (typeof(T) == typeof(Player))
            {
                return player as T;
            }
            return null;
        }
    }
}
