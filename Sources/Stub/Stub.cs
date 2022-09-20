using Model;

namespace Stub
{
    public class Stub
    {
        public List<Player> LoadPlayers()
        {
            List<Player> list = new List<Player>();
            list.Add(new Player("name 1"));
            list.Add(new Player("name 2"));
            list.Add(new Player("name 3"));
            list.Add(new Player("name 4"));
            list.Add(new Player("name 5"));
            list.Add(new Player("name 6"));
            return list;
        }

        public List<Die> LoadDices()
        {
            List<Die> list = new List<Die>();
            list.Add(new Die("Dice 1"));
            list.Add(new Die("Dice 1"));
            list.Add(new Die("Dice 1"));
            list.Add(new Die("Dice 1"));
            list.Add(new Die("Dice 1"));
            list.Add(new Die("Dice 1"));

            return list;
        }

        public List<NumberDieFace> LoadNumFaces()
        {
            List<NumberDieFace> list = new List<NumberDieFace>();
            list.Add(new NumberDieFace(1));
            list.Add(new NumberDieFace(2));
            list.Add(new NumberDieFace(3));
            list.Add(new NumberDieFace(4));
            list.Add(new NumberDieFace(5));
            list.Add(new NumberDieFace(6));
            list.Add(new NumberDieFace(7));

            return list;
        }

        public List<ColorDieFace> LoadClrFaces()
        {
            List<ColorDieFace> list = new List<ColorDieFace>();
            list.Add(new ColorDieFace("#fff"));
            list.Add(new ColorDieFace("#fff66"));
            list.Add(new ColorDieFace("#fff11"));
            list.Add(new ColorDieFace("#fff22"));
            list.Add(new ColorDieFace("#fff33"));
            list.Add(new ColorDieFace("#fff44"));
            list.Add(new ColorDieFace("#fff55"));

            return list;
        }

        public List<ImageDieFace> LoadImgFaces()
        {
            List<ImageDieFace> list = new List<ImageDieFace>();
            list.Add(new ImageDieFace("http://baseUrl/img/1"));
            list.Add(new ImageDieFace("http://baseUrl/img/2"));
            list.Add(new ImageDieFace("http://baseUrl/img/3"));
            list.Add(new ImageDieFace("http://baseUrl/img/4"));
            list.Add(new ImageDieFace("http://baseUrl/img/5"));
            list.Add(new ImageDieFace("http://baseUrl/img/6"));
            list.Add(new ImageDieFace("http://baseUrl/img/7"));

            return list;
        }


    }
}