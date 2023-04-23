import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class UserSettingsComponent implements OnInit {

  engineOptions: DropDownOption[] = [
    {
      id: 0,
      name: "Human"
    },
    {
      id: 1,
      name: "Heuristic"
    },
    {
      id: 2,
      name: "MCTS"
    },
    {
      id: 3,
      name: "MCTS with AMAF"
    },
    {
      id: 4,
      name: "MCTS with Savebridge"
    },
    {
      id: 5,
      name: "MCTS with AMAF and Savebridge"
    }
  ]

  variantOptions = [
    {
      id: 0,
      name: "Basic"
    },
    {
      id: 1,
      name: "SWAP"
    },
  ]

  variantValue: number;
  player1Value: number;
  player2Value: number;
  delayValue: number;

  constructor() {

  }

  ngOnInit(): void {
    this.setDefaultValues();
  }

  setDefaultValues() {

    this.player1Value = 0;
    this.player2Value = 5;
    this.variantValue = 0;
    this.delayValue = 1000;

  }

  onDelayValueChange() {
    if (this.delayValue < 500) this.delayValue = 500;
    if (this.delayValue > 10000) this.delayValue = 10000;
  }
}

interface DropDownOption {
  id: number;
  name: string;
}
