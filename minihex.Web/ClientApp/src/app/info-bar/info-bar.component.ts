import { Component, ViewEncapsulation } from '@angular/core';
import { BoardService } from '../board/board.service';
import { PlayerColor, WinningPathInfo } from '../communication/communication.service';
import { ErrorInterceptor } from '../error-interceptor/error-interceptor.service';
import { UserSettingsService } from '../user-settings/user-settings.service';

@Component({
  selector: 'info-bar',
  templateUrl: './info-bar.component.html',
  styleUrls: ['./info-bar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoBarComponent {

  player1Option = "";
  player2Option = "";

  visiblePvp = false;
  lockStatus = false;

  moveStatus = ""

  moveStatusOption = [
    "Your move!",
    "Waiting for {0} move...",
    "Error occured: {0}",
    "Game finished. {0} won!"
    ]

  constructor(private boardService: BoardService,
    private userSettingsService: UserSettingsService,
    private errorInterceptor: ErrorInterceptor) {
    this.boardService.boardSetupEvent.subscribe(() => {
      this.lockStatus = false;
      this.player1Option = this.userSettingsService.engineOptions.find(e =>
        e.id == this.userSettingsService.currentSettings.player1Variant as number)?.name ?? "";

      this.player2Option = this.userSettingsService.engineOptions.find(e =>
        e.id == this.userSettingsService.currentSettings.player2Variant as number)?.name ?? "";

      this.visiblePvp = true;

      if (this.boardService.gameConfig.playerInvolve && this.boardService.gameConfig.playerStart) {
        this.moveStatus = this.moveStatusOption[0];
      }
      else {
        this.moveStatus = this.moveStatusOption[1].replace("{0}", this.player1Option);
      }

      this.boardService.outMoveEvent.subscribe(() => {
        if (this.lockStatus) return;
        if (this.boardService.nextColor == PlayerColor.Black) {
          if (this.boardService.gameConfig.playerInvolve && this.boardService.gameConfig.playerStart) {
            this.moveStatus = this.moveStatusOption[0];
          }
          else {
            this.moveStatus = this.moveStatusOption[1].replace("{0}", this.player1Option);
          }
        }
        else {
          if (this.boardService.gameConfig.playerInvolve && !this.boardService.gameConfig.playerStart) {
            this.moveStatus = this.moveStatusOption[0];
          }
          else {
            this.moveStatus = this.moveStatusOption[1].replace("{0}", this.player2Option);
          }
        }
      })
    })

    this.boardService.finishGameEvent.subscribe((info: WinningPathInfo) => {
      if (this.lockStatus) return;
      let playerWon = info.colorWon == PlayerColor.Black ? this.player2Option : this.player1Option;
      this.moveStatus = this.moveStatusOption[3].replace("{0}", playerWon);
    })

    this.errorInterceptor.serverErrorEvent.subscribe((error: string) => {
      this.lockStatus = true;
      this.moveStatus = this.moveStatusOption[2].replace("{0}", error.substr(0, 50));
    })

  }

}
