using ClassificationTree.Infraestructure.Base;
using NUnit.Framework;
using System.Collections.Generic;

namespace NLPDT.Test
{
    public class Tests
    {
        private NlpDT.NLPDT nlp;
        [SetUp]
        public void Setup() {
            
        }
        public Tests() {
            nlp = new NlpDT.NLPDT();
        }

        private void CompareList(IList<(IFunction.function, string)> result, IList<(IFunction.function, string)> expected) {
            Assert.AreEqual(expected.Count, result.Count, "Number the elements is not equal");

            for (int i = 0; i < result.Count; i++) {
                Assert.AreEqual(expected[i].Item1, result[i].Item1, $"The Item1 (function) in the position {i} is not same");
                Assert.AreEqual(expected[i].Item2?.ToLowerInvariant(), result[i].Item2?.ToLowerInvariant(), $"The Item2 (value) in the position {i} is not same");
            }
        }

        [Test]
        public void Test0() {
            var result0 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alki ordenado por precio extendido?");
            IList<(IFunction.function, string)> expected0 = new List<(IFunction.function, string)>();
            expected0.Add((IFunction.function.TableSearchField, "precio"));
            expected0.Add((IFunction.function.TableSearchField, "extendido"));
            expected0.Add((IFunction.function.TableField, "ciudad"));
            expected0.Add((IFunction.function.Filter, "Berlin"));
            expected0.Add((IFunction.function.Conjunction, "AND"));
            expected0.Add((IFunction.function.TableField, "cliente"));
            expected0.Add((IFunction.function.Filter, "Alki"));
            expected0.Add((IFunction.function.Order, "ASC"));
            expected0.Add((IFunction.function.TableOrderField, "precio"));
            expected0.Add((IFunction.function.TableOrderField, "extendido"));
            CompareList(result0, expected0);
        }
        [Test]
        public void Test1() {
            var result1 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alki?");
            IList<(IFunction.function, string)> expected1 = new List<(IFunction.function, string)>();
            expected1.Add((IFunction.function.TableSearchField, "precio"));
            expected1.Add((IFunction.function.TableSearchField, "extendido"));
            expected1.Add((IFunction.function.TableField, "ciudad"));
            expected1.Add((IFunction.function.Filter, "Berlin"));
            expected1.Add((IFunction.function.Conjunction, "AND"));
            expected1.Add((IFunction.function.TableField, "cliente"));
            expected1.Add((IFunction.function.Filter, "Alki"));
            CompareList(result1, expected1);
        }
        [Test]
        public void Test2() {
            var result2 = nlp.SentenceTest("¿Me puedes decir la edades de Juan y la de Maria?");
            IList<(IFunction.function, string)> expected2 = new List<(IFunction.function, string)>();
            expected2.Add((IFunction.function.None, null));
            expected2.Add((IFunction.function.TableSearchField, "edad"));
            expected2.Add((IFunction.function.Filter, "Juan"));
            expected2.Add((IFunction.function.Conjunction, "AND"));
            expected2.Add((IFunction.function.Filter, "Maria"));
            CompareList(result2, expected2);
        }
        [Test]
        public void Test3() {
            var result3 = nlp.SentenceTest("¿Que edad tiene Juan?");
            IList<(IFunction.function, string)> expected3 = new List<(IFunction.function, string)>();
            expected3.Add((IFunction.function.TableSearchField, "edad"));
            expected3.Add((IFunction.function.None, null));
            expected3.Add((IFunction.function.Filter, "Juan"));
            CompareList(result3, expected3);
        }
        [Test]
        public void Test4() {
            var result4 = nlp.SentenceTest("¿Cual es la edad y apellido de las personas llamada Juan y Maria?");
            IList<(IFunction.function, string)> expected4 = new List<(IFunction.function, string)>();
            expected4.Add((IFunction.function.TableSearchField, "edad"));
            expected4.Add((IFunction.function.Conjunction, "AND"));
            expected4.Add((IFunction.function.TableSearchField, "apellido"));
            expected4.Add((IFunction.function.TableField, "persona"));
            //expected4.Add((IFunction.function.TableField, "llamado"));
            expected4.Add((IFunction.function.Filter, "Juan"));
            expected4.Add((IFunction.function.Conjunction, "AND"));
            expected4.Add((IFunction.function.Filter, "Maria"));
            CompareList(result4, expected4);
        }
        [Test]
        public void Test5() {
            var result5 = nlp.SentenceTest("¿Que edad tiene cuyo nombre sea Juan?");
            IList<(IFunction.function, string)> expected5 = new List<(IFunction.function, string)>();
            expected5.Add((IFunction.function.TableSearchField, "edad"));
            expected5.Add((IFunction.function.TableField, "nombre"));
            expected5.Add((IFunction.function.None, null));
            expected5.Add((IFunction.function.Filter, "Juan"));
            CompareList(result5, expected5);
        }
        [Test]
        public void Test6() {
            var result6 = nlp.SentenceTest("¿Me puedes decir el apellido de Juan?");
            IList<(IFunction.function, string)> expected6 = new List<(IFunction.function, string)>();
            expected6.Add((IFunction.function.None, null));
            expected6.Add((IFunction.function.TableSearchField, "apellido"));
            expected6.Add((IFunction.function.Filter, "Juan"));
            CompareList(result6, expected6);
        }
        [Test]
        public void Test7() {
            var result7 = nlp.SentenceTest("¿Cual es la edad de la persona llamada Juan?");
            IList<(IFunction.function, string)> expected7 = new List<(IFunction.function, string)>();
            expected7.Add((IFunction.function.TableSearchField, "edad"));
            expected7.Add((IFunction.function.TableField, "persona"));
            expected7.Add((IFunction.function.TableField, "llamado"));
            expected7.Add((IFunction.function.Filter, "Juan"));
            CompareList(result7, expected7);
        }
        [Test]
        public void Test8() {
            var result8 = nlp.SentenceTest("¿Me puedes decir el apellido y sueldo de Juan?");
            IList<(IFunction.function, string)> expected8 = new List<(IFunction.function, string)>();
            expected8.Add((IFunction.function.None, null));
            expected8.Add((IFunction.function.TableSearchField, "apellido"));
            expected8.Add((IFunction.function.Conjunction, "AND"));
            expected8.Add((IFunction.function.TableSearchField, "sueldo"));
            expected8.Add((IFunction.function.Filter, "Juan"));
            CompareList(result8, expected8);
        }
        [Test]
        public void Test9() {
            var result9 = nlp.SentenceTest("¿Me puedes decir el apellido, sueldo y dirección de Juan?");
            IList<(IFunction.function, string)> expected9 = new List<(IFunction.function, string)>();
            expected9.Add((IFunction.function.None, null));
            expected9.Add((IFunction.function.TableSearchField, "apellido"));
            expected9.Add((IFunction.function.Punct, ","));
            expected9.Add((IFunction.function.TableSearchField, "sueldo"));
            expected9.Add((IFunction.function.Conjunction, "AND"));
            expected9.Add((IFunction.function.TableSearchField, "dirección"));
            expected9.Add((IFunction.function.Filter, "Juan"));
            CompareList(result9, expected9);
        }
        [Test]
        public void Test10() {
            var result10 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alfki ordenado por precio extendido?");
            IList<(IFunction.function, string)> expected10 = new List<(IFunction.function, string)>();
            expected10.Add((IFunction.function.TableSearchField, "precio"));
            expected10.Add((IFunction.function.TableSearchField, "extendido"));
            expected10.Add((IFunction.function.TableField, "ciudad"));
            expected10.Add((IFunction.function.Filter, "Berlin"));
            expected10.Add((IFunction.function.Conjunction, "AND"));
            expected10.Add((IFunction.function.TableField, "cliente"));
            expected10.Add((IFunction.function.Filter, "Alfki"));
            expected10.Add((IFunction.function.Order, "ASC"));
            expected10.Add((IFunction.function.TableOrderField, "precio"));
            expected10.Add((IFunction.function.TableOrderField, "extendido"));
            CompareList(result10, expected10);
        }
        [Test]
        public void Test11() {
            var result11 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente por precio extendido?");
            IList<(IFunction.function, string)> expected11 = new List<(IFunction.function, string)>();
            expected11.Add((IFunction.function.TableSearchField, "precio"));
            expected11.Add((IFunction.function.TableSearchField, "extendido"));
            expected11.Add((IFunction.function.TableField, "ciudad"));
            expected11.Add((IFunction.function.Filter, "Berlin"));
            expected11.Add((IFunction.function.Conjunction, "AND"));
            expected11.Add((IFunction.function.TableField, "cliente"));
            expected11.Add((IFunction.function.Filter, "Alfki"));
            expected11.Add((IFunction.function.Order, "DESC"));
            expected11.Add((IFunction.function.TableOrderField, "precio"));
            expected11.Add((IFunction.function.TableOrderField, "extendido"));
            CompareList(result11, expected11);
        }
        [Test]
        public void Test12() {
            var result12 = nlp.SentenceTest("¿Cual es el precio extendido y el nombre del cliente cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente por precio extendido y ordenado ascendentemente por nombre del cliente?");
            IList<(IFunction.function, string)> expected12 = new List<(IFunction.function, string)>();
            expected12.Add((IFunction.function.TableSearchField, "precio"));
            expected12.Add((IFunction.function.TableSearchField, "extendido"));
            expected12.Add((IFunction.function.Conjunction, "AND"));
            expected12.Add((IFunction.function.TableSearchField, "nombre"));
            expected12.Add((IFunction.function.TableSearchField, "cliente"));
            expected12.Add((IFunction.function.TableField, "ciudad"));
            expected12.Add((IFunction.function.Filter, "Berlin"));
            expected12.Add((IFunction.function.Conjunction, "AND"));
            expected12.Add((IFunction.function.TableField, "cliente"));
            expected12.Add((IFunction.function.Filter, "Alfki"));
            expected12.Add((IFunction.function.Order, "DESC"));
            expected12.Add((IFunction.function.TableOrderField, "precio"));
            expected12.Add((IFunction.function.TableOrderField, "extendido"));
            expected12.Add((IFunction.function.Conjunction, "AND"));
            expected12.Add((IFunction.function.Order, "ASC"));
            expected12.Add((IFunction.function.TableOrderField, "nombre"));
            expected12.Add((IFunction.function.TableOrderField, "cliente"));
            CompareList(result12, expected12);
        }
        [Test]
        public void Test13() {
            var result13 = nlp.SentenceTest("¿Cuántas horas ha trabajado Juan el mes de Enero?");
            IList<(IFunction.function, string)> expected13 = new List<(IFunction.function, string)>();
            expected13.Add((IFunction.function.TableSearchField, "hora"));
            expected13.Add((IFunction.function.None, null));
            expected13.Add((IFunction.function.Filter, "Juan"));
            expected13.Add((IFunction.function.TableField, "mes"));
            expected13.Add((IFunction.function.Filter, "Enero"));
            CompareList(result13, expected13);
        }
        [Test]
        public void Test14() {
            var result14 = nlp.SentenceTest("¿Cuántos días ha faltado Juan en el mes de Enero?");
            IList<(IFunction.function, string)> expected14 = new List<(IFunction.function, string)>();
            expected14.Add((IFunction.function.TableSearchField, "día"));
            expected14.Add((IFunction.function.None, null));
            expected14.Add((IFunction.function.Filter, "Juan"));
            expected14.Add((IFunction.function.TableField, "mes"));
            expected14.Add((IFunction.function.Filter, "Enero"));
            CompareList(result14, expected14);
        }
        [Test]
        public void Test15() {
            //var result15 = nlp.SentenceTest("¿En el mes de enero cuantos dias ha faltado Jose Maria?");
            //IList<(IFunction.function, string)> expected15 = new List<(IFunction.function, string)>();
            //expected15.Add((IFunction.function.TableSearchField, "día"));
            //expected15.Add((IFunction.function.TableField, "mes"));
            //expected15.Add((IFunction.function.Filter, "Enero"));
            //expected15.Add((IFunction.function.None, null));
            //expected15.Add((IFunction.function.Filter, "Jose"));
            //expected15.Add((IFunction.function.Filter, "Maria"));              
            //CompareList(result15, expected15);
        }
        [Test]
        public void Test16() {
            var result16 = nlp.SentenceTest("¿Me puedes decir el apellido con un sueldo de 8000€?");
            IList<(IFunction.function, string)> expected16 = new List<(IFunction.function, string)>();
            expected16.Add((IFunction.function.None, null));
            expected16.Add((IFunction.function.TableSearchField, "apellido"));
            expected16.Add((IFunction.function.TableField, "sueldo"));
            expected16.Add((IFunction.function.Filter, "8000"));
            CompareList(result16, expected16);
        }
        [Test]
        public void Test17() {
            var result17 = nlp.SentenceTest("¿Me puedes decir el apellido con un sueldo de 8000 ?");
            IList<(IFunction.function, string)> expected17 = new List<(IFunction.function, string)>();
            expected17.Add((IFunction.function.None, null));
            expected17.Add((IFunction.function.TableSearchField, "apellido"));
            expected17.Add((IFunction.function.TableField, "sueldo"));
            expected17.Add((IFunction.function.Filter, "8000"));
            CompareList(result17, expected17);
        }
        [Test]
        public void Test18() {
            var result18 = nlp.SentenceTest("¿Me puedes decir el apellido con un sueldo de 8000€?");
            IList<(IFunction.function, string)> expected18 = new List<(IFunction.function, string)>();
            expected18.Add((IFunction.function.None, null));
            expected18.Add((IFunction.function.TableSearchField, "apellido"));
            expected18.Add((IFunction.function.TableField, "sueldo"));
            expected18.Add((IFunction.function.Filter, "8000"));
            CompareList(result18, expected18);
        }
        [Test]
        public void Test19() {
            var result19 = nlp.SentenceTest("¿Nombre y apellidos de personas con un sueldo de 18000?");
            IList<(IFunction.function, string)> expected19 = new List<(IFunction.function, string)>();
            expected19.Add((IFunction.function.TableSearchField, "Nombre"));
            expected19.Add((IFunction.function.Conjunction, "AND"));
            expected19.Add((IFunction.function.TableSearchField, "apellido"));
            expected19.Add((IFunction.function.TableField, "persona"));
            expected19.Add((IFunction.function.TableField, "sueldo"));
            expected19.Add((IFunction.function.Filter, "18000"));
            CompareList(result19, expected19);
        }
        [Test]
        public void Test20() {
            var result20 = nlp.SentenceTest("¿Nombre y apellidos de personas con 18 años?");
            IList<(IFunction.function, string)> expected20 = new List<(IFunction.function, string)>();
            expected20.Add((IFunction.function.TableSearchField, "nombre"));
            expected20.Add((IFunction.function.Conjunction, "AND"));
            expected20.Add((IFunction.function.TableSearchField, "apellido"));
            expected20.Add((IFunction.function.TableField, "persona"));
            expected20.Add((IFunction.function.TableField, "año"));
            expected20.Add((IFunction.function.Filter, "18"));
            CompareList(result20, expected20);
        }
        [Test]
        public void Test21() {
            var result21 = nlp.SentenceTest("¿Nombre y apellidos de personas que tengan 18 años?");
            IList<(IFunction.function, string)> expected21 = new List<(IFunction.function, string)>();
            expected21.Add((IFunction.function.TableSearchField, "nombre"));
            expected21.Add((IFunction.function.Conjunction, "AND"));
            expected21.Add((IFunction.function.TableSearchField, "apellido"));
            expected21.Add((IFunction.function.TableField, "persona"));
            expected21.Add((IFunction.function.TableField, "año"));
            expected21.Add((IFunction.function.Filter, "18"));
            CompareList(result21, expected21);
        }
        [Test]
        public void Test22() {
            var result22 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alki ordenado descendentemente por precio extendido?");
            IList<(IFunction.function, string)> expected22 = new List<(IFunction.function, string)>();
            expected22.Add((IFunction.function.TableSearchField, "precio"));
            expected22.Add((IFunction.function.TableSearchField, "extendido"));
            expected22.Add((IFunction.function.TableField, "ciudad"));
            expected22.Add((IFunction.function.Filter, "Berlin"));
            expected22.Add((IFunction.function.Conjunction, "AND"));
            expected22.Add((IFunction.function.TableField, "cliente"));
            expected22.Add((IFunction.function.Filter, "Alki"));
            expected22.Add((IFunction.function.Order, "DESC"));
            expected22.Add((IFunction.function.TableOrderField, "precio"));
            expected22.Add((IFunction.function.TableOrderField, "extendido"));
            CompareList(result22, expected22);
        }
        [Test]
        public void Test23() {
            var result23 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alki ordenado descendentemente por precio extendido y ordenado ascendentemente por nombre?");
            IList<(IFunction.function, string)> expected23 = new List<(IFunction.function, string)>();
            expected23.Add((IFunction.function.TableSearchField, "precio"));
            expected23.Add((IFunction.function.TableSearchField, "extendido"));
            expected23.Add((IFunction.function.TableField, "ciudad"));
            expected23.Add((IFunction.function.Filter, "Berlin"));
            expected23.Add((IFunction.function.Conjunction, "AND"));
            expected23.Add((IFunction.function.TableField, "cliente"));
            expected23.Add((IFunction.function.Filter, "Alki"));
            expected23.Add((IFunction.function.Order, "DESC"));
            expected23.Add((IFunction.function.TableOrderField, "precio"));
            expected23.Add((IFunction.function.TableOrderField, "extendido"));
            expected23.Add((IFunction.function.Conjunction, "AND"));
            expected23.Add((IFunction.function.Order, "ASC"));
            expected23.Add((IFunction.function.TableOrderField, "nombre"));
            CompareList(result23, expected23);
        }
        [Test]
        public void Test24() {
            var result24 = nlp.SentenceTest("¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alki ordenado descendentemente por precio extendido y ascendentemente por nombre");
            IList<(IFunction.function, string)> expected24 = new List<(IFunction.function, string)>();
            expected24.Add((IFunction.function.TableSearchField, "precio"));
            expected24.Add((IFunction.function.TableSearchField, "extendido"));
            expected24.Add((IFunction.function.TableField, "ciudad"));
            expected24.Add((IFunction.function.Filter, "Berlin"));
            expected24.Add((IFunction.function.Conjunction, "AND"));
            expected24.Add((IFunction.function.TableField, "cliente"));
            expected24.Add((IFunction.function.Filter, "Alki"));
            expected24.Add((IFunction.function.Order, "DESC"));
            expected24.Add((IFunction.function.TableOrderField, "precio"));
            expected24.Add((IFunction.function.TableOrderField, "extendido"));
            expected24.Add((IFunction.function.Conjunction, "AND"));
            expected24.Add((IFunction.function.Order, "ASC"));
            expected24.Add((IFunction.function.TableOrderField, "nombre"));
            CompareList(result24, expected24);
        }
        [Test]
        public void Test25() {
            var result25 = nlp.SentenceTest("¿Nombre y apellidos que tengan menos de 18 años?");
            IList<(IFunction.function, string)> expected25 = new List<(IFunction.function, string)>();
            expected25.Add((IFunction.function.TableSearchField, "nombre"));
            expected25.Add((IFunction.function.Conjunction, "AND"));
            expected25.Add((IFunction.function.TableSearchField, "apellido"));
            expected25.Add((IFunction.function.TableField, "año"));
            expected25.Add((IFunction.function.Operator, "<"));
            expected25.Add((IFunction.function.Filter, "18"));
            CompareList(result25, expected25);
        }
        [Test]
        public void Test26() {
            var result26 = nlp.SentenceTest("¿Nombre y apellidos que tengan igual a 18 años?");
            IList<(IFunction.function, string)> expected26 = new List<(IFunction.function, string)>();
            expected26.Add((IFunction.function.TableSearchField, "nombre"));
            expected26.Add((IFunction.function.Conjunction, "AND"));
            expected26.Add((IFunction.function.TableSearchField, "apellido"));
            expected26.Add((IFunction.function.TableField, "año"));
            expected26.Add((IFunction.function.Operator, "="));
            expected26.Add((IFunction.function.Filter, "18"));
            CompareList(result26, expected26);
        }
        [Test]
        public void Test27() {
            var result27 = nlp.SentenceTest("¿Nombre y apellidos que tengan más de 18 años?");
            IList<(IFunction.function, string)> expected27 = new List<(IFunction.function, string)>();
            expected27.Add((IFunction.function.TableSearchField, "nombre"));
            expected27.Add((IFunction.function.Conjunction, "AND"));
            expected27.Add((IFunction.function.TableSearchField, "apellido"));
            expected27.Add((IFunction.function.TableField, "año"));
            expected27.Add((IFunction.function.Operator, ">"));
            expected27.Add((IFunction.function.Filter, "18"));
            CompareList(result27, expected27);
        }
        [Test]
        public void Test28() {
            var result28 = nlp.SentenceTest("¿Nombre y apellidos menores de 18 años?");
            IList<(IFunction.function, string)> expected28 = new List<(IFunction.function, string)>();
            expected28.Add((IFunction.function.TableSearchField, "nombre"));
            expected28.Add((IFunction.function.Conjunction, "AND"));
            expected28.Add((IFunction.function.TableSearchField, "apellido"));
            expected28.Add((IFunction.function.Operator, "<"));
            expected28.Add((IFunction.function.TableField, "año"));
            expected28.Add((IFunction.function.Filter, "18"));
            CompareList(result28, expected28);
        }
        [Test]
        public void Test29() {
            var result29 = nlp.SentenceTest("¿Nombre y apellidos igual a 18 años?");
            IList<(IFunction.function, string)> expected29 = new List<(IFunction.function, string)>();
            expected29.Add((IFunction.function.TableSearchField, "nombre"));
            expected29.Add((IFunction.function.Conjunction, "AND"));
            expected29.Add((IFunction.function.TableSearchField, "apellido"));
            expected29.Add((IFunction.function.Operator, "="));
            expected29.Add((IFunction.function.TableField, "año"));
            expected29.Add((IFunction.function.Filter, "18"));
            CompareList(result29, expected29);
        }
        [Test]
        public void Test30() {
            var result30 = nlp.SentenceTest("¿Nombre y apellidos mayores de 18 años?");
            IList<(IFunction.function, string)> expected30 = new List<(IFunction.function, string)>();
            expected30.Add((IFunction.function.TableSearchField, "nombre"));
            expected30.Add((IFunction.function.Conjunction, "AND"));
            expected30.Add((IFunction.function.TableSearchField, "apellido"));
            expected30.Add((IFunction.function.Operator, ">"));
            expected30.Add((IFunction.function.TableField, "año"));
            expected30.Add((IFunction.function.Filter, "18"));
            CompareList(result30, expected30);
        }
        [Test]
        public void Test31() {
            var result31 = nlp.SentenceTest("¿Nombre del cliente que tenga un precio extendido igual que 16?");
            IList<(IFunction.function, string)> expected31 = new List<(IFunction.function, string)>();
            expected31.Add((IFunction.function.TableSearchField, "nombre"));
            expected31.Add((IFunction.function.TableSearchField, "cliente"));
            expected31.Add((IFunction.function.TableField, "precio"));
            expected31.Add((IFunction.function.TableField, "extendido"));
            expected31.Add((IFunction.function.Operator, "="));
            expected31.Add((IFunction.function.Filter, "16"));
            CompareList(result31, expected31);
        }
        [Test]
        public void Test32() {
            var result32 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido mayor que 16?");
            IList<(IFunction.function, string)> expected32 = new List<(IFunction.function, string)>();
            expected32.Add((IFunction.function.TableSearchField, "nombre"));
            expected32.Add((IFunction.function.TableSearchField, "cliente"));
            expected32.Add((IFunction.function.TableField, "precio"));
            expected32.Add((IFunction.function.TableField, "extendido"));
            expected32.Add((IFunction.function.Operator, ">"));
            expected32.Add((IFunction.function.Filter, "16"));
            CompareList(result32, expected32);
        }
        [Test]
        public void Test33() {
            var result33 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido menor a 60?");
            IList<(IFunction.function, string)> expected33 = new List<(IFunction.function, string)>();
            expected33.Add((IFunction.function.TableSearchField, "nombre"));
            expected33.Add((IFunction.function.TableSearchField, "cliente"));
            expected33.Add((IFunction.function.TableField, "precio"));
            expected33.Add((IFunction.function.TableField, "extendido"));
            expected33.Add((IFunction.function.Operator, "<"));
            expected33.Add((IFunction.function.Filter, "60"));
            CompareList(result33, expected33);
        }
        [Test]
        public void Test34() {
            var result34 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido mayor o igual que 16?");
            IList<(IFunction.function, string)> expected34 = new List<(IFunction.function, string)>();
            expected34.Add((IFunction.function.TableSearchField, "nombre"));
            expected34.Add((IFunction.function.TableSearchField, "cliente"));
            expected34.Add((IFunction.function.TableField, "precio"));
            expected34.Add((IFunction.function.TableField, "extendido"));
            expected34.Add((IFunction.function.Operator, ">"));
            expected34.Add((IFunction.function.Conjunction, "OR"));
            expected34.Add((IFunction.function.Operator, "="));
            expected34.Add((IFunction.function.Filter, "16"));
            CompareList(result34, expected34);
        }
        [Test]
        public void Test35() {
            var result35 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido menor o igual que 16?");
            IList<(IFunction.function, string)> expected35 = new List<(IFunction.function, string)>();
            expected35.Add((IFunction.function.TableSearchField, "nombre"));
            expected35.Add((IFunction.function.TableSearchField, "cliente"));
            expected35.Add((IFunction.function.TableField, "precio"));
            expected35.Add((IFunction.function.TableField, "extendido"));
            expected35.Add((IFunction.function.Operator, "<"));
            expected35.Add((IFunction.function.Conjunction, "OR"));
            expected35.Add((IFunction.function.Operator, "="));
            expected35.Add((IFunction.function.Filter, "16"));
            CompareList(result35, expected35);
        }
        [Test]
        public void Test36() {
            var result36 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido superior a 16?");
            IList<(IFunction.function, string)> expected36 = new List<(IFunction.function, string)>();
            expected36.Add((IFunction.function.TableSearchField, "nombre"));
            expected36.Add((IFunction.function.TableSearchField, "cliente"));
            expected36.Add((IFunction.function.TableField, "precio"));
            expected36.Add((IFunction.function.TableField, "extendido"));
            expected36.Add((IFunction.function.Operator, ">"));
            expected36.Add((IFunction.function.Filter, "16"));
            CompareList(result36, expected36);
        }
        [Test]
        public void Test37() {
            var result37 = nlp.SentenceTest("¿Nombre del cliente que tiene un precio extendido inferior a 16?");
            IList<(IFunction.function, string)> expected37 = new List<(IFunction.function, string)>();
            expected37.Add((IFunction.function.TableSearchField, "nombre"));
            expected37.Add((IFunction.function.TableSearchField, "cliente"));
            expected37.Add((IFunction.function.TableField, "precio"));
            expected37.Add((IFunction.function.TableField, "extendido"));
            expected37.Add((IFunction.function.Operator, "<"));
            expected37.Add((IFunction.function.Filter, "16"));
            CompareList(result37, expected37);
        }
        [Test]
        public void Test38() {
            var result38 = nlp.SentenceTest("¿Nombre y apellidos de personas que tengan 18 años?");
        }
        [Test]
        public void Test39() {
            var result39 = nlp.SentenceTest("¿Nombre y apellidos de todas las personas que tengan 18 años?");
        }
        [Test]
        public void Test40() {
            var result40 = nlp.SentenceTest("¿Nombre del cliente con el precio extendido de 10?");
        }
        [Test]
        public void Test41() {
            var result41 = nlp.SentenceTest("¿Nombre y apellidos tengan 18 años?");
        }
    }
}