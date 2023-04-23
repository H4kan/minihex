import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { CommunicationService, GameStatus, NextMoveInfo, PlayerColor, WinningPathInfo } from '../communication/communication.service';

@Injectable({
  providedIn: 'root',
})
export class BoardService {

  gameId = 0;
  gameStatus = GameStatus.InProgress;




  public outMoveEvent = new Subject<number>();
  public finishGameEvent = new Subject<WinningPathInfo>();

  public nextColor: PlayerColor = PlayerColor.Black;

  public gameConfig: HexGameConfig;

  public moveCounter = 0;

  constructor(private communicationService: CommunicationService) {
    this.loadConfig({
      size: 11,
      playerInvolve: true,
      playerStart: true,
      swapVariant: true,
      callInterval: 1000
    });
  }

  makeMove(fieldIdx: number, serverSide: boolean) {
    this.nextColor = this.nextColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    this.moveCounter++;
    this.outMoveEvent.next(fieldIdx);
    if (!serverSide) {
      this.communicationService.makeNextMove(this.gameId, fieldIdx, this.moveCounter)
        .then((info: NextMoveInfo) => {
          if (info.status == GameStatus.Finished) {
            this.gameStatus = info.status;
            this.finishGame();
          }
          else {
            if (this.shouldServerMove()) {
              setTimeout(() => {
                this.getServerMove()
              }, this.gameConfig.callInterval);
            }
          }
        })
    }
    else if (this.shouldServerMove()) {
      setTimeout(() => {
        this.getServerMove()
      }, this.gameConfig.callInterval);
    }
  }

  getServerMove() {
    this.communicationService.getNextMove(this.gameId, this.moveCounter)
      .then((info: NextMoveInfo) => {
        this.gameStatus = info.status;
        this.makeMove(info.fieldIdx, true);
        if (this.gameStatus == GameStatus.Finished) this.finishGame();
      })

  }

  shouldServerMove(): boolean {
    return this.gameStatus == GameStatus.InProgress &&
      !this.gameConfig.playerInvolve ||
      (this.gameConfig.playerStart && this.nextColor == PlayerColor.White) ||
      (!this.gameConfig.playerStart && this.nextColor == PlayerColor.Black);
  }

  finishGame() {
    this.communicationService.getWinningPath(this.gameId)
      .then((info: WinningPathInfo) => {
        this.finishGameEvent.next(info);
      })
  }

  loadConfig(config: HexGameConfig) {
    this.gameConfig = config;
  }
}

export interface HexGameConfig {

  size: number;
  playerInvolve: boolean;
  playerStart: boolean;
  swapVariant: boolean;
  callInterval: number;
}





