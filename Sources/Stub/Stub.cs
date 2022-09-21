using Model;

namespace Stub
{
    public class Stub
    {
        public List<Player> LoadPlayers()
        {
            List<Player> list = new()
            {
                new Player("name 1"),
                new Player("name 2"),
                new Player("name 3"),
                new Player("name 4"),
                new Player("name 5"),
                new Player("name 6")
            };
            return list;
        }

        public List<Die> LoadDices()
        {
            List<Die> list = new()
            {
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1")
            };

            return list;
        }

        public List<NumberDieFace> LoadNumFaces()
        {
            List<NumberDieFace> list = new()
            {
                new NumberDieFace(1),
                new NumberDieFace(2),
                new NumberDieFace(3),
                new NumberDieFace(4),
                new NumberDieFace(5),
                new NumberDieFace(6),
                new NumberDieFace(7)
            };

            return list;
        }

        public List<ColorDieFace> LoadClrFaces()
        {
            List<ColorDieFace> list = new()
            {
                new ColorDieFace("ffffff"),
                new ColorDieFace("ffff66"),
                new ColorDieFace("ffff11"),
                new ColorDieFace("ffff22"),
                new ColorDieFace("ffff33"),
                new ColorDieFace("ffff44"),
                new ColorDieFace("ffff55")
            };

            return list;
        }

        public List<ImageDieFace> LoadImgFaces()
        {
            List<ImageDieFace> list = new()
            {
                new ImageDieFace("http://baseUrl/img/1"),
                new ImageDieFace("http://baseUrl/img/2"),
                new ImageDieFace("http://baseUrl/img/3"),
                new ImageDieFace("http://baseUrl/img/4"),
                new ImageDieFace("http://baseUrl/img/5"),
                new ImageDieFace("http://baseUrl/img/6"),
                new ImageDieFace("http://baseUrl/img/7")
            };

            return list;
        }
    }
}