import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { GameVariant, Algorithm } from '../communication/communication.service';
import { UserSettingsService } from './user-settings.service';

@Component({
  selector: 'user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class UserSettingsComponent implements OnInit {

  engineOptions = this.userSettingsService.engineOptions;
  variantOptions = this.userSettingsService.variantOptions;

  variantValue: number;
  player1Value: number;
  player2Value: number;
  delayValue: number;
  sizeValue: number;

  constructor(private userSettingsService: UserSettingsService) {

  }

  ngOnInit(): void {
    this.setDefaultValues();
  }

  setDefaultValues() {

    this.player1Value = 0;
    this.player2Value = 5;
    this.variantValue = 0;
    this.delayValue = 1000;
    this.sizeValue = 8;

  }

  onDelayValueChange() {
    if (this.delayValue < 500) this.delayValue = 500;
    if (this.delayValue > 10000) this.delayValue = 10000;
  }

  onSizeValueChange() {
    if (this.sizeValue < 5) this.sizeValue = 5;
    if (this.sizeValue > 11) this.sizeValue = 11;
  }

  submit() {
    this.userSettingsService.submitSettings({

      player1Variant: this.player1Value as Algorithm,
      player2Variant: this.player2Value as Algorithm,
      variant: this.variantValue as GameVariant,
      delay: this.delayValue,
      size: this.sizeValue

      })
  }

}


