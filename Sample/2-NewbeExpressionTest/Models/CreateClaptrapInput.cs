using _2_NewbeExpressionTest.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _2_NewbeExpressionTest.Models
{
    public class CreateClaptrapInput
    {
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public string NickName { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Age { get; set; }

        public int[] Scores { get; set; }

        public List<string> Subjects { get; set; }

        public IEnumerable<int> Items { get; set; }

        [EnumRang]
        public StudentLevel Level { get; set; }
    }

    public enum StudentLevel
    {
        Good,
        Excellent,
        General,
        Poor
    }
}