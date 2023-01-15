using System;
using System.Collections.Generic;
using Gameplay.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        public event Action AttackRequested;

        [SerializeField] private Button _attackButton;
        [SerializeField] private Transform _infoPanels;

        private readonly List<BuffPanel> _buffPanels = new();
        private readonly List<StatPanel> _statPanels = new();
        private BuffPanel.Factory _buffPanelFactory;
        private StatPanel.Factory _statPanelFactory;


        [Inject]
        public void Construct(
            StatPanel.Factory statPanelFactory,
            BuffPanel.Factory buffPanelFactory)
        {
            _statPanelFactory = statPanelFactory;
            _buffPanelFactory = buffPanelFactory;
        }

        private void Start()
        {
            _attackButton.OnClickAsObservable()
                .Subscribe(_ => AttackRequested?.Invoke())
                .AddTo(this);
        }

        public void ActivateAttackButton(bool value)
        {
            _attackButton.interactable = value;
        }

        public void AddBuffPanel(Buff buff)
        {
            var buffPanel = _buffPanelFactory.Create(buff);
            buffPanel.transform.SetParent(_infoPanels);
            _buffPanels.Add(buffPanel);
        }

        public void AddStatPanel(
            IReactiveProperty<float> reactiveProperty,
            string statIconAssetId)
        {
            var statPanel = _statPanelFactory.Create(
                reactiveProperty,
                statIconAssetId);
            statPanel.transform.SetParent(_infoPanels);
            _statPanels.Add(statPanel);
        }

        public void ClearPanels()
        {
            foreach (var statPanel in _statPanels)
                statPanel.Expire();
            _statPanels.Clear();

            foreach (var buffPanel in _buffPanels)
                buffPanel.Expire();
            _buffPanels.Clear();
        }
    }
}