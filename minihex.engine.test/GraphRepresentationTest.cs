using FluentAssertions;
using minihex.engine.Model;

namespace minihex.engine.test
{
    [TestClass]
    public class GraphRepresentationTest
    {
        private GraphRepresentation _whiteRepresentation;
        private GraphRepresentation _blackRepresentation;

        [TestInitialize]
        public void Initialize()
        {
            var size = 5;

            _whiteRepresentation = new GraphRepresentation(size, Model.Enums.PlayerColor.White);
            _blackRepresentation = new GraphRepresentation(size, Model.Enums.PlayerColor.Black);
        }

        #region Game Finish Tests
        [TestMethod]
        public void ShouldFinishGame_Easy()
        {
            _whiteRepresentation.IsGameFinished().Should().BeFalse();
            _blackRepresentation.IsGameFinished().Should().BeFalse();

            _whiteRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(1, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(2, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(3, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(4, Model.Enums.PlayerColor.White);

            _blackRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(5, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(10, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(15, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(20, Model.Enums.PlayerColor.Black);

            _whiteRepresentation.IsGameFinished().Should().BeTrue();
            _blackRepresentation.IsGameFinished().Should().BeTrue();
        }

        [TestMethod]
        public void ShouldFinishGame_Hard()
        {
            _whiteRepresentation.IsGameFinished().Should().BeFalse();

            _whiteRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(1, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(5, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(2, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(6, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(7, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(11, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(10, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(12, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(13, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(17, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(16, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(18, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(19, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(15, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(14, Model.Enums.PlayerColor.White);


            _whiteRepresentation.IsGameFinished().Should().BeTrue();
        }

        [TestMethod]
        public void ShouldNotFinishGame_Easy()
        {
            _whiteRepresentation.IsGameFinished().Should().BeFalse();
            _blackRepresentation.IsGameFinished().Should().BeFalse();

            _whiteRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(1, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(2, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(3, Model.Enums.PlayerColor.White);


            _whiteRepresentation.ColorVerticeAndReduce(5, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(6, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(7, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(8, Model.Enums.PlayerColor.White);

            _whiteRepresentation.ColorVerticeAndReduce(10, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(11, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(12, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(13, Model.Enums.PlayerColor.White);

            _whiteRepresentation.ColorVerticeAndReduce(15, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(16, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(17, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(18, Model.Enums.PlayerColor.White);

            _whiteRepresentation.ColorVerticeAndReduce(4, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(9, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(14, Model.Enums.PlayerColor.Black);
            _whiteRepresentation.ColorVerticeAndReduce(19, Model.Enums.PlayerColor.Black);



            _blackRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(5, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(10, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(15, Model.Enums.PlayerColor.Black);

            _blackRepresentation.ColorVerticeAndReduce(1, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(6, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(11, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(16, Model.Enums.PlayerColor.Black);

            _blackRepresentation.ColorVerticeAndReduce(2, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(7, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(12, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(17, Model.Enums.PlayerColor.Black);

            _blackRepresentation.ColorVerticeAndReduce(3, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(8, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(13, Model.Enums.PlayerColor.Black);
            _blackRepresentation.ColorVerticeAndReduce(18, Model.Enums.PlayerColor.Black);


            _whiteRepresentation.IsGameFinished().Should().BeFalse();
            _blackRepresentation.IsGameFinished().Should().BeFalse();
        }

        [TestMethod]
        public void ShouldNotFinishGame_Hard()
        {
            _whiteRepresentation.IsGameFinished().Should().BeFalse();

            _whiteRepresentation.ColorVerticeAndReduce(0, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(1, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(2, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(3, Model.Enums.PlayerColor.White);


            _whiteRepresentation.ColorVerticeAndReduce(9, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(14, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(13, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(12, Model.Enums.PlayerColor.White);
            _whiteRepresentation.ColorVerticeAndReduce(11, Model.Enums.PlayerColor.White);


            _whiteRepresentation.IsGameFinished().Should().BeFalse();
        }
        #endregion
    }
}