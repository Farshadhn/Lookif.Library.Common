using Microsoft.EntityFrameworkCore;
using System;

namespace Lookif.Library.Common.Attributes.Database;
public class OnDelete : Attribute //This is Used To support Cascade Delete,etc
{
    public DeleteBehavior deleteBehavior { get; set; } = DeleteBehavior.Restrict;
}
