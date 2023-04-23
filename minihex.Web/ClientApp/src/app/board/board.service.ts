import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Algorithm, CommunicationService, GameStatus, GameVariant, NextMoveInfo, PlayerColor, WinningPathInfo } from '../communication/communication.service';
import { UserSettingsService } from '../user-settings/user-settings.service';

@Injectable({
  providedIn: 'root',
})
export class BoardService {

  gameId = "";
  gameStatus = GameStatus.InProgress;



  public startGameEvent = new Subject();
  public outMoveEvent = new Subject<number>();
  public boardSetupEvent = new Subject<number>();
  public finishGameEvent = new Subject<WinningPathInfo>();

  public nextColor: PlayerColor = PlayerColor.Black;

  public gameConfig: HexGameConfig;

  public moveCounter = 0;

  constructor(private communicationService: CommunicationService,
    private userSettingsService: UserSettingsService) {

    this.userSettingsService.settingsSubmittedEvent.subscribe((gameId: string) => {
      this.gameId = gameId;
      this.outMoveEvent = new Subject<number>();
      var currentSettings = this.userSettingsService.currentSettings;
      this.loadConfig({
        size: currentSettings.size,
        swapVariant: currentSettings.variant == GameVariant.SWAP,
        playerStart: currentSettings.player1Variant == Algorithm.Human,
        callInterval: currentSettings.delay,
        playerInvolve: currentSettings.player1Variant == Algorithm.Human ||
          currentSettings.player2Variant == Algorithm.Human
      })
      this.boardSetupEvent.next(0);
    })
  }

  makeMove(fieldIdx: number, serverSide: boolean) {
    this.nextColor = this.nextColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    this.outMoveEvent.next(fieldIdx);
    if (!serverSide) {
      this.communicationService.makeNextMove(this.gameId, fieldIdx, ++this.moveCounter)
        .then((info: NextMoveInfo) => {
          if (this.gameId !== info.gameId) return;
          if (info.status == GameStatus.Finished) {
            this.gameStatus = info.status;
            this.finishGame();
          }
          else {
            var currGameId = this.gameId;
            if (this.shouldServerMove()) {
              setTimeout(() => {
                this.getServerMove(currGameId)
              }, this.gameConfig.callInterval);
            }
          }
        })
    }
    else if (this.shouldServerMove()) {
      var currGameId = this.gameId;
      setTimeout(() => {
        this.getServerMove(currGameId)
      }, this.gameConfig.callInterval);
    }
  }

  getServerMove(gameId: string) {
    this.communicationService.getNextMove(gameId, ++this.moveCounter)
      .then((info: NextMoveInfo) => {
        if (this.gameId === info.gameId) {
          this.gameStatus = info.status;
          this.makeMove(info.fieldIdx, true);
          if (this.gameStatus == GameStatus.Finished) this.finishGame();
        }
      })

  }

  shouldServerMove(): boolean {
    return this.gameStatus == GameStatus.InProgress &&
      (!this.gameConfig.playerInvolve ||
      (this.gameConfig.playerStart && this.nextColor == PlayerColor.White) ||
      (!this.gameConfig.playerStart && this.nextColor == PlayerColor.Black));
  }

  finishGame() {
    this.communicationService.getWinningPath(this.gameId)
      .then((info: WinningPathInfo) => {
        if (this.gameId == info.gameId) {
          this.finishGameEvent.next(info);
          this.gameId = "";
        }
      })
  }

  loadConfig(config: HexGameConfig) {
    this.gameConfig = config;
    this.moveCounter = 0;
    this.nextColor = PlayerColor.Black;
    this.startGameEvent.next(0);
  }
}

export interface HexGameConfig {

  size: number;
  playerInvolve: boolean;
  playerStart: boolean;
  swapVariant: boolean;
  callInterval: number;
}





