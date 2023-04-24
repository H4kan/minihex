import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { CommunicationService, UserSettings } from '../communication/communication.service';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsService {


  currentSettings: UserSettings;

  settingsSubmittedEvent = new Subject<string>();

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

  constructor(private communicationService: CommunicationService) {

  }


  submitSettings(newSettings: UserSettings) {

    this.communicationService.beginGame(newSettings)
      .then((gameId: string) => {
        this.currentSettings = newSettings;
        this.settingsSubmittedEvent.next(gameId);
      });

  }
}

interface DropDownOption {
  id: number;
  name: string;
}

