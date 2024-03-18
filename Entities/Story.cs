using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformancePaging.Entities;

public class Story
{
    [Key]
    [Column("story_id")]
    public int StoryId { get; set; }
    [Column("story_headline")]
    [StringLength(2000)]
    [Unicode(false)]
    public string StoryHeadline { get; set; } = null!;
    [Column("story_subheadline")]
    [StringLength(2000)]
    [Unicode(false)]
    public string? StorySubheadline { get; set; }
    [Column("story_byline")]
    [StringLength(255)]
    [Unicode(false)]
    public string? StoryByline { get; set; }
    [Column("story_summary")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? StorySummary { get; set; }
}
